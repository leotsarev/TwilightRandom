using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using Twilight.Domain;

namespace Twilight.Web
{
    public class GameRepository
    {
        private readonly DbContext dbContext;

        public GameRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private async Task<Game?> LoadGameByPredicate(Expression<Func<Game, bool>> predicate)
        {
            return await GameSelector().FirstOrDefaultAsync(predicate);
        }

        private IIncludableQueryable<Game, Player> GameSelector()
        {
            return dbContext.Games
                            .Include(g => g.PlayerSlots)
                            .ThenInclude(p => p.SelectedFaction)
                            .Include(g => g.PlayerSlots)
                            .ThenInclude(p => p.PossibleFactions)
                            .Include(g => g.PlayerSlots)
                            .ThenInclude(p => p.Player);
        }

        public Task<Game?> LoadGameBySlug(string slug) => LoadGameByPredicate(g => g.Slug == slug);
        public Task<Game?> LoadGameById(int id) => LoadGameByPredicate(g => g.Id == id);

        public Task<Game?> LoadLastGameOrDefault() => GameSelector().OrderByDescending(g => g.Id).FirstOrDefaultAsync();
    }
}
