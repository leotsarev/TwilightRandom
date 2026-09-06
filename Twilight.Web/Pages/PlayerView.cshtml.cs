using JoinRpg.Common.PrimitiveTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Twilight.Domain;

namespace Twilight.Web.Pages
{
    public class PlayerViewModel : PageModel
    {
        private readonly IGameRepository gameRepository;

        public PlayerViewModel(IGameRepository gameRepository)
        {
            this.gameRepository = gameRepository;
        }

        [BindProperty(SupportsGet = true)]
        public int JoinrpgUserId { get; set; }

        public Player Player { get; set; } = null!;

        public List<Game> Games { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var joinrpgUserId = new UserIdentification(JoinrpgUserId);

            var player = await gameRepository.LoadPlayerByJoinrpgUserId(joinrpgUserId);
            if (player is null)
            {
                return NotFound();
            }

            Player = player;
            Games = await gameRepository.LoadGamesForPlayer(joinrpgUserId);

            return Page();
        }
    }
}
