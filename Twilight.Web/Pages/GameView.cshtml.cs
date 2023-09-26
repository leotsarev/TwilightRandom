using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Twilight.Domain;
using Twilight.Web.Migrations;

namespace Twilight.Web.Pages
{
    public class GameViewModel : PageModel
    {
        private readonly GameRepository gameRepository;
        private readonly DbContext dbContext;

        public GameViewModel(GameRepository gameRepository, DbContext dbContext)
        {
            this.gameRepository = gameRepository;
            this.dbContext = dbContext;
        }
        [BindProperty(SupportsGet = true)]
        public string? Slug { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? Id { get; set; }

        public bool AdminMode { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SlotSlug { get; set; }

        public int? SlotId { get; set; }

        public Game Game { get; set; } = null!;

        public bool AllSelected { get; set; }

        public bool Alliances { get; set; }

        public List<Domain.Faction> UnUsedFactions { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            (var game, AdminMode) = await LoadGameAsync();

            if (game is null)
            {
                return NotFound();
            }
            Game = game;

            foreach (var slot in game.PlayerSlots)
            {
                if (slot.Slug == SlotSlug)
                {
                    SlotId = slot.Id;
                }
            }

            AllSelected = game.PlayerSlots.All(p => p.SelectedFaction is not null);
            Alliances = game.PlayerSlots.Any(p => p.AlliedWith is not null);

            var possibleFactions = await dbContext.Factions.ToListAsync();

            if (AllSelected || AdminMode)
            {
                UnUsedFactions = possibleFactions.ExceptAlreadyUsedIn(game).ToList();
            }
            else
            {
                UnUsedFactions = possibleFactions.ExceptAlreadyUsedInForUser(game, SlotId).ToList();
            }

            return Page();

        }

        public async Task<(Game? game, bool adminMode)> LoadGameAsync()
        {
            if (Slug is not null)
            {
                var game = await gameRepository.LoadGameBySlug(Slug);
                if (game is not null)
                {
                    return (game, true);
                }
            }
            if (Id is not null)
            {
                return (await gameRepository.LoadGameById(Id.Value), false);
            }
            return (null, false);
        }
    }
}
