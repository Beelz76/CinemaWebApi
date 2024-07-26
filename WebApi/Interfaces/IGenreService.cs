namespace WebApi.Interfaces
{
    public interface IGenreService
    {
        Task<bool> CreateGenreAsync(string name);
        Task<IReadOnlyList<Contracts.Genre>> GetGenresAsync();
        Task<bool> UpdateGenreAsync(Guid genreUid, string name);
        Task<bool> DeleteGenreAsync(Guid genreUid);
        Task<bool> GenreExistsAsync(string name);
        bool IsValidGenreName(string name);
    }
}