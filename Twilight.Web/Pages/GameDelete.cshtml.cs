using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Twilight.Dal;
using Twilight.Domain;
using Twilight.Web.Auth;
using JoinRpg.Common.WebInfrastructure.Auth;

namespace Twilight.Web.Pages;

[Authorize]
public class GameDeleteModel(IGameRepository gameRepository, TwilightDbContext dbContext) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public Game Game { get; set; } = null!;

    public bool CanDelete { get; set; }

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

        Game = game;
        CanDelete = HasNoSelections(game);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
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
        if (!HasNoSelections(game))
        {
            Game = game;
            CanDelete = false;
            return Page();
        }

        dbContext.Games.Remove(game);
        await dbContext.SaveChangesAsync();

        return RedirectToPage("Index");
    }

    private static bool HasNoSelections(Game game) => game.PlayerSlots.All(s => s.SelectedFaction is null);
}
