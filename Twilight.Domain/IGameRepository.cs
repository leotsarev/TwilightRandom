namespace Twilight.Domain
{
    public interface IGameRepository
    {
        Task<Game?> LoadGameById(int id);
        Task<Game?> LoadGameBySlug(string slug);
        Task<Game?> LoadLastGameOrDefault();
    }
}