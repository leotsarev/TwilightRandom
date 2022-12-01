using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Composition.Convention;
using Twilight.Domain;

namespace Twilight.Web.Pages
{
    public class GameViewModel : PageModel
    {
        private readonly GameRepository gameRepository;

        public GameViewModel(GameRepository gameRepository)
        {
            this.gameRepository = gameRepository;
        }
        [BindProperty(SupportsGet = true)]
        public string? Slug { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? Id { get; set; }

        public bool AdminMode { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SlotSlug { get; set; }

        public int? SlotId { get; set; }

        public Game Game { get; set; }

        public bool AllSelected { get; set; }

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
