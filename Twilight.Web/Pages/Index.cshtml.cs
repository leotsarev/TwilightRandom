using Microsoft.AspNetCore.Mvc.RazorPages;
using Twilight.Domain;

namespace Twilight.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IGameRepository gameRepository;

        public IndexModel(IGameRepository gameRepository)
        {
            this.gameRepository = gameRepository;
        }

        public List<Game> Games { get; set; } = new();

        public async Task OnGetAsync()
        {
            Games = await gameRepository.LoadAllGames();
        }
    }
}
