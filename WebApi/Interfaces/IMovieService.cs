using WebApi.Contracts;

namespace WebApi.Interfaces
{
    public interface IMovieService
    {
        Task<bool> CreateMovieAsync(MovieDto movieDto);
        Task<IReadOnlyList<MovieDto>> GetAllMoviesAsync();
        Task<MovieDto> GetSingleMovieAsync(Guid movieUid);
        Task<IReadOnlyList<MovieInfoDto>> GetMoviesInfoAsync();
        Task<bool> UpdateMovieAsync(Guid movieUid, MovieDto movieDto);
        Task<bool> DeleteMovieAsync(Guid movieUid);
        Task<bool> MovieExistsAsync(Guid movieUid);
        Task<bool> MovieExistsByTitleAsync(string movieTitle);
        Task<bool> MovieExistsByInfoAsync(MovieDto movieDto, Guid? movieUid = null);
        bool IsValidMovieTitle(string title);
        bool IsValidNamesInList(List<string> list);
    }
}