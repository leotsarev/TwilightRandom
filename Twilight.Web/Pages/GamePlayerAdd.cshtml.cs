using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Twilight.Dal;
using Twilight.Domain;
using Twilight.Web.Auth;
using Twilight.Web.GamePlayers;
using JoinRpg.Common.WebInfrastructure.Auth;
using TwilightRandom;

namespace Twilight.Web.Pages;

[Authorize]
public class GamePlayerAddModel(IGameRepository gameRepository, TwilightDbContext dbContext) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    [Required]
    [BindProperty]
    public string Name { get; set; } = "";

    [Range(2, 3)]
    [BindProperty]
    public int FactionsCount { get; set; } = 2;

    public Game Game { get; set; } = null!;

    [TempData]
    public string? Message { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var game = await gameRepository.LoadGameById(Id);
        if (game is null)
        {
            return NotFound();
        }

        var guardResult = await CheckGuardAsync(game);
        if (guardResult is not null)
        {
            return guardResult;
        }

        Game = game;
        FactionsCount = game.PlayerSlots.FirstOrDefault()?.PossibleFactions.Count ?? 2;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var game = await gameRepository.LoadGameById(Id);
        if (game is null)
        {
            return NotFound();
        }

        var guardResult = await CheckGuardAsync(game);
        if (guardResult is not null)
        {
            return guardResult;
        }

        if (!ModelState.IsValid || string.IsNullOrWhiteSpace(Name))
        {
            Game = game;
            return Page();
        }

        var allFactions = await dbContext.Factions.ToListAsync();
        var freeFactions = allFactions.ExceptAlreadyUsedIn(game).ToList();
        var usedColors = game.PlayerSlots.Select(s => s.Color);

        var playerInput = PlayerLineParser.Parse(Name.Trim());
        var player = await PlayerResolver.ResolvePlayerAsync(dbContext, playerInput.Name, playerInput.JoinrpgUserId);
        var result = Randomiser.AddPlayer(player, usedColors, freeFactions, FactionsCount);
        game.PlayerSlots.Add(PlayerSlotFactory.CreateSlot(result));

        await dbContext.SaveChangesAsync();

        return RedirectToPage("GameView", new { Id });
    }

    private async Task<IActionResult?> CheckGuardAsync(Game game)
    {
        if (!GameAuthorization.CanManage(game, User.TryGetJoinrpgUserId()))
        {
            return NotFound();
        }

        if (game.Status is not (GameStatus.ChoosingSides or GameStatus.Planned))
        {
            Message = "Нельзя добавить игрока: игра уже завершена";
            return RedirectToPage("GameView", new { Id });
        }

        if (game.PlayerSlots.Any(s => s.AlliedWith is not null))
        {
            Message = "Нельзя добавить игрока: в игре включён режим союзов";
            return RedirectToPage("GameView", new { Id });
        }

        var maxPlayers = Enum.GetValues<PlayerColor>().Length;
        if (game.PlayerSlots.Count >= maxPlayers)
        {
            Message = $"В игре уже максимальное количество игроков ({maxPlayers})";
            return RedirectToPage("GameView", new { Id });
        }

        var allFactions = await dbContext.Factions.ToListAsync();
        if (allFactions.ExceptAlreadyUsedIn(game).Count() < 3)
        {
            Message = "Недостаточно свободных фракций (нужно минимум 3)";
            return RedirectToPage("GameView", new { Id });
        }

        return null;
    }
}
