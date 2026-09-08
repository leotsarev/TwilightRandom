using JoinRpg.Common.PrimitiveTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Twilight.Dal;
using JoinRpg.Common.WebInfrastructure.Auth;

namespace Twilight.Web.Pages.Admin;

[Authorize]
public class SetGameDatesModel(TwilightDbContext dbContext) : PageModel
{
    private static readonly UserIdentification AdminUserId = new(1);

    public List<UndatedGame> UndatedGames { get; set; } = [];

    [TempData]
    public string? Message { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        if (User.GetJoinrpgUserId() != AdminUserId)
        {
            return NotFound();
        }

        await LoadUndatedGames();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int gameId, DateOnly date)
    {
        if (User.GetJoinrpgUserId() != AdminUserId)
        {
            return NotFound();
        }

        var game = await dbContext.Games.FindAsync(gameId);
        if (game is null)
        {
            Message = "Игра не найдена, обновите страницу";
            return RedirectToPage();
        }

        game.Date = date;
        await dbContext.SaveChangesAsync();

        Message = $"Игре {game.Name} проставлена дата {date:yyyy-MM-dd}";
        return RedirectToPage();
    }

    private async Task LoadUndatedGames()
    {
        UndatedGames = await dbContext.Games
            .Where(g => g.Date == null)
            .OrderBy(g => g.Id)
            .Select(g => new UndatedGame(g.Id, g.Name))
            .ToListAsync();
    }

    public record UndatedGame(int Id, string Name);
}
