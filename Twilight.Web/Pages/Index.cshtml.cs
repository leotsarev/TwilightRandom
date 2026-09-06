using JoinRpg.Common.PrimitiveTypes;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Twilight.Domain;
using JoinRpg.Common.WebInfrastructure.Auth;

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

        public UserIdentification? MyJoinrpgUserId { get; set; }

        public async Task OnGetAsync()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                MyJoinrpgUserId = User.GetJoinrpgUserId();
                MyGames = await gameRepository.LoadGamesForPlayer(MyJoinrpgUserId!);
            }
        }
    }
}
