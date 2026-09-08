using JoinRpg.Common.PrimitiveTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Twilight.Dal;
using Twilight.Domain;
using JoinRpg.Common.WebInfrastructure.Auth;

namespace Twilight.Web.Pages.Admin;

[Authorize]
public class MergePlayersModel(TwilightDbContext dbContext) : PageModel
{
    private static readonly UserIdentification AdminUserId = new(1);

    public List<UnmappedPlayer> UnmappedPlayers { get; set; } = [];

    [TempData]
    public string? Message { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        if (User.GetJoinrpgUserId() != AdminUserId)
        {
            return NotFound();
        }

        await LoadUnmappedPlayers();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int playerId, int newJoinrpgUserId)
    {
        if (User.GetJoinrpgUserId() != AdminUserId)
        {
            return NotFound();
        }

        var sourcePlayer = await dbContext.Players.FindAsync(playerId);
        if (sourcePlayer is null || sourcePlayer.JoinrpgUserId is not null)
        {
            Message = "Игрок не найден или уже смапплен, обновите страницу";
            return RedirectToPage();
        }

        var targetUserId = new UserIdentification(newJoinrpgUserId);
        var targetPlayer = await dbContext.Players.FirstOrDefaultAsync(p => p.JoinrpgUserId == targetUserId);

        if (targetPlayer is null)
        {
            sourcePlayer.JoinrpgUserId = targetUserId;
            await dbContext.SaveChangesAsync();
            Message = $"Игроку {sourcePlayer.Name} проставлен joinrpgUserId {newJoinrpgUserId}";
            return RedirectToPage();
        }

        if (targetPlayer.Id == sourcePlayer.Id)
        {
            return RedirectToPage();
        }

        var sourceSlots = await dbContext.Set<PlayerSlot>().Where(s => s.PlayerId == sourcePlayer.Id).ToListAsync();
        var targetGameIds = await dbContext.Set<PlayerSlot>().Where(s => s.PlayerId == targetPlayer.Id).Select(s => s.GameId).ToListAsync();

        var conflictingSlot = sourceSlots.FirstOrDefault(s => targetGameIds.Contains(s.GameId));
        if (conflictingSlot is not null)
        {
            Message = $"Не удалось смаппить: игроки {sourcePlayer.Name} и {targetPlayer.Name} оба участвуют в игре {conflictingSlot.GameId}. Разберитесь вручную.";
            return RedirectToPage();
        }

        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        foreach (var slot in sourceSlots)
        {
            slot.PlayerId = targetPlayer.Id;
        }

        var createdGames = await dbContext.Games.Where(g => g.CreatedByPlayerId == sourcePlayer.Id).ToListAsync();
        foreach (var game in createdGames)
        {
            game.CreatedByPlayerId = targetPlayer.Id;
        }

        dbContext.Players.Remove(sourcePlayer);

        await dbContext.SaveChangesAsync();
        await transaction.CommitAsync();

        Message = $"Игрок {sourcePlayer.Name} смапплен на {targetPlayer.Name} (joinrpgUserId {newJoinrpgUserId}), игры перенесены";
        return RedirectToPage();
    }

    private async Task LoadUnmappedPlayers()
    {
        var players = await dbContext.Players.Where(p => p.JoinrpgUserId == null).ToListAsync();
        var gameCounts = await dbContext.Set<PlayerSlot>()
            .Where(s => players.Select(p => p.Id).Contains(s.PlayerId))
            .GroupBy(s => s.PlayerId)
            .Select(g => new { PlayerId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(g => g.PlayerId, g => g.Count);

        UnmappedPlayers = players
            .Select(p => new UnmappedPlayer(p.Id, p.Name, p.AvatarUrl, gameCounts.GetValueOrDefault(p.Id)))
            .Where(p => p.GameCount > 0)
            .OrderByDescending(p => p.GameCount)
            .ToList();
    }

    public record UnmappedPlayer(int Id, string Name, string? AvatarUrl, int GameCount);
}
