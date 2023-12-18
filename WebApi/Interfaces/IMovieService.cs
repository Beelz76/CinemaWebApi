namespace WebApi.Interface
{
    public interface IMovieService
    {
        bool CreateMovie(Contracts.MovieInfo movieInfo);
        List<Contracts.Movie>? GetAllMovies();
        Contracts.Movie? GetSingleMovie(Guid movieUid);
        List<Contracts.MovieInfo>? GetMoviesInfo();
        bool UpdateMovie(Guid movieUid, Contracts.MovieInfo movieInfo);
        bool DeleteMovie(Guid movieUid);
        bool IsMovieExists(Guid movieUid);
        bool CheckMovieTitle(string movieTitle);
        bool CheckMovieInfo(Contracts.MovieInfo movieInfo);
        bool CheckMovieInfo(Guid movieUid, Contracts.MovieInfo movieInfo);
        bool CheckRegex(string name);
        bool CheckRegexList(List<string> list);
    }
}