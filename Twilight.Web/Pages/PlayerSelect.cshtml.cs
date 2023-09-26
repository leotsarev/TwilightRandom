using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using Twilight.Domain;

namespace Twilight.Web.Pages
{
    public class PlayerSelectModel : PageModel
    {
        private readonly GameRepository gameRepository;
        private readonly DbContext dbContext;

        public PlayerSelectModel(GameRepository gameRepository, DbContext dbContext)
        {
            this.gameRepository = gameRepository;
            this.dbContext = dbContext;
        }
        [BindProperty()]
        public int GameId { get; set; }

        [BindProperty()]
        public int? FactionId { get; set; }

        [BindProperty()]
        public string? SlotSlug { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var game = await gameRepository.LoadGameById(GameId);
            var player = game?.PlayerSlots.FirstOrDefault(s => s.Slug == SlotSlug);
            if (player is null || game is null)
            {
                return RedirectToPage("GameCreate", new { Id = GameId });
            }
            var faction = player.PossibleFactions.FirstOrDefault(f => f.Id == FactionId);
            if (faction is null)
            {
                var possibleFactions = await dbContext.Factions.ToListAsync();
                faction = possibleFactions.ExceptAlreadyUsedIn(game).Shuffle().First();
            }

            player.SelectedFaction = faction;

            await dbContext.SaveChangesAsync();

            return RedirectToPage("GameView", new { Id = GameId, SlotSlug = player.Slug });
        }
    }
}
