using JoinRpg.Common.WebInfrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Twilight.Dal;
using Twilight.Domain;
using Twilight.Web.Auth;

namespace Twilight.Web.Pages;

[Authorize]
public class GameFinishModel(IGameRepository gameRepository, TwilightDbContext dbContext) : PageModel
{
    private const int WinningPoints = 14;

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

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
        if (!GameAuthorization.CanManage(game, User.TryGetJoinrpgUserId()))
        {
            return NotFound();
        }

        if (game.PlayerSlots.Any(s => s.SelectedFaction is null))
        {
            Message = "Нельзя завершить игру: не все игроки выбрали фракцию";
            return RedirectToPage("GameView", new { Id });
        }

        Game = game;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Dictionary<int, int?> points, string name, string? notes)
    {
        var game = await gameRepository.LoadGameById(Id);
        if (game is null)
        {
            return NotFound();
        }
        if (!GameAuthorization.CanManage(game, User.TryGetJoinrpgUserId()))
        {
            return NotFound();
        }

        if (game.PlayerSlots.Any(s => s.SelectedFaction is null))
        {
            Message = "Нельзя завершить игру: не все игроки выбрали фракцию";
            return RedirectToPage("GameView", new { Id });
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            Message = "Название игры не может быть пустым";
            return RedirectToPage(new { Id });
        }

        var slotPoints = new Dictionary<int, int>();
        foreach (var slot in game.PlayerSlots)
        {
            if (!points.TryGetValue(slot.Id, out var value) || value is null || value < 0 || value > WinningPoints)
            {
                Message = "Нужно ввести очки от 0 до 14 для каждого игрока";
                return RedirectToPage(new { Id });
            }
            slotPoints[slot.Id] = value.Value;
        }

        var winners = game.PlayerSlots.Where(s => slotPoints[s.Id] == WinningPoints).ToList();
        if (winners.Count != 1)
        {
            Message = "Ровно один игрок должен набрать 14 очков";
            return RedirectToPage(new { Id });
        }

        foreach (var slot in game.PlayerSlots)
        {
            slot.Points = slotPoints[slot.Id];
            slot.IsWinner = slot == winners[0];
        }
        game.Name = name;
        game.Notes = string.IsNullOrWhiteSpace(notes) ? null : notes;
        game.Status = GameStatus.Played;

        await dbContext.SaveChangesAsync();

        return RedirectToPage("GameView", new { Id });
    }
}
