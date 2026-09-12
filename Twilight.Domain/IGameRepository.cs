using JoinRpg.Common.PrimitiveTypes;

namespace Twilight.Domain
{
    public interface IGameRepository
    {
        Task<Game?> LoadGameById(int id);
        Task<Game?> LoadLastGameOrDefault();
        Task<List<Game>> LoadGamesForPlayer(UserIdentification joinrpgUserId);
        Task<List<Game>> LoadAllGames();
        Task<Player?> LoadPlayerByJoinrpgUserId(UserIdentification joinrpgUserId);
    }
}
