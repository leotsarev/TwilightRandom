using Microsoft.AspNetCore.Mvc.RazorPages;
using Twilight.Domain;
using Twilight.Web.Auth;

namespace Twilight.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IGameRepository gameRepository;

        public IndexModel(IGameRepository gameRepository)
        {
            this.gameRepository = gameRepository;
        }

        public List<Game> MyGames { get; set; } = new();

        public async Task OnGetAsync()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                MyGames = await gameRepository.LoadGamesForPlayer(User.GetJoinrpgUserId());
            }
        }

        public Faction? GetMyFaction(Game game)
        {
            var joinrpgUserId = User.GetJoinrpgUserId();
            return game.PlayerSlots
                .FirstOrDefault(ps => ps.Player.JoinrpgUserId == joinrpgUserId)
                ?.SelectedFaction;
        }
    }
}
