using JoinRpg.Common.WebInfrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Twilight.Dal;
using Twilight.Domain;
using Twilight.Web.Auth;

namespace Twilight.Web.Pages;

[Authorize]
public class GameCancelModel(IGameRepository gameRepository, TwilightDbContext dbContext) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public Game Game { get; set; } = null!;

    public bool CanCancel { get; set; }

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
        CanCancel = CanBeCancelled(game);
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
        if (!CanBeCancelled(game))
        {
            Game = game;
            CanCancel = false;
            return Page();
        }

        game.Status = GameStatus.Cancelled;
        await dbContext.SaveChangesAsync();

        return RedirectToPage("Index");
    }

    private static bool CanBeCancelled(Game game) =>
        game.Status != GameStatus.Played
        && game.Status != GameStatus.Cancelled;
}
