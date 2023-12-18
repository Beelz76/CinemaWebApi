namespace WebApi.Interface
{
    public interface IGenreService
    {
        bool CreateGenre(string name);
        List<Contracts.Genre>? GetGenres();
        bool UpdateGenre(Guid genreUid, string name);
        bool DeleteGenre(Guid genreUid);
        bool IsGenreExists(Guid genreUid);
        bool CheckGenreName(string name);
        bool CheckRegex(string name);
    }
}