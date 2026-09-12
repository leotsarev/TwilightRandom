using System.ComponentModel.DataAnnotations;
using JoinRpg.Common.WebInfrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Twilight.Dal;
using Twilight.Domain;
using Twilight.Web.Auth;
using Twilight.Web.GamePlayers;

namespace Twilight.Web.Pages;

[Authorize]
public class GamePlayerChangeModel(IGameRepository gameRepository, TwilightDbContext dbContext) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    [BindProperty(SupportsGet = true)]
    public int SlotId { get; set; }

    [Required]
    [BindProperty]
    public string Name { get; set; } = "";

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

        var guardResult = CheckGuard(game, out var slot);
        if (guardResult is not null)
        {
            return guardResult;
        }

        Game = game;
        Name = slot!.Player.Name;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var game = await gameRepository.LoadGameById(Id);
        if (game is null)
        {
            return NotFound();
        }

        var guardResult = CheckGuard(game, out var slot);
        if (guardResult is not null)
        {
            return guardResult;
        }

        if (!ModelState.IsValid || string.IsNullOrWhiteSpace(Name))
        {
            Game = game;
            return Page();
        }

        var playerInput = PlayerInput.Parse(Name.Trim());
        slot!.Player = await PlayerResolver.ResolvePlayerAsync(dbContext, playerInput.Name, playerInput.JoinrpgUserId);
        await dbContext.SaveChangesAsync();

        return RedirectToPage("GameView", new { Id });
    }

    private IActionResult? CheckGuard(Game game, out PlayerSlot? slot)
    {
        slot = game.PlayerSlots.FirstOrDefault(s => s.Id == SlotId);
        if (slot is null)
        {
            return NotFound();
        }

        if (!GameAuthorization.CanManage(game, User.TryGetJoinrpgUserId()))
        {
            return NotFound();
        }

        if (game.Status == GameStatus.Played)
        {
            Message = "Нельзя сменить игрока: игра уже прошла";
            return RedirectToPage("GameView", new { Id });
        }

        return null;
    }
}
