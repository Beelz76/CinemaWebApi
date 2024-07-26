namespace WebApi.Interfaces
{
    public interface IMovieService
    {
        Task<bool> CreateMovieAsync(Contracts.MovieInfo movieInfo);
        Task<IReadOnlyList<Contracts.Movie>> GetAllMoviesAsync();
        Task<Contracts.Movie> GetSingleMovieAsync(Guid movieUid);
        Task<IReadOnlyList<Contracts.MovieInfo>> GetMoviesInfoAsync();
        Task<bool> UpdateMovieAsync(Guid movieUid, Contracts.MovieInfo movieInfo);
        Task<bool> DeleteMovieAsync(Guid movieUid);
        Task<bool> MovieExistsAsync(Guid movieUid);
        Task<bool> MovieExistsByTitleAsync(string movieTitle);
        Task<bool> MovieExistsByInfoAsync(Contracts.MovieInfo movieInfo, Guid? movieUid = null);
        bool IsValidMovieTitle(string title);
        bool IsValidNamesInList(List<string> list);
    }
}