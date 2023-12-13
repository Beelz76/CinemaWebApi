using DatabaseAccessLayer;
using DatabaseAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services
{
    public class MovieService
    {
        private readonly CinemaDbContext _cinemaDbContext;

        public MovieService(CinemaDbContext cinemaDbContext)
        {
            _cinemaDbContext = cinemaDbContext;
        }

        public bool CreateMovie(Contracts.MovieInfo movieInfo)
        {
            var movie = new Movie
            {
                MovieUid = Guid.NewGuid(),
                Title = movieInfo.Title,
                ReleaseYear = movieInfo.ReleaseYear,
                Duration = movieInfo.Duration,
                Description = movieInfo.Description
            };

            AddDirectors(movieInfo.Directors, movie);
            AddCountries(movieInfo.Countries, movie);
            AddGenres(movieInfo.Genres, movie);

            _cinemaDbContext.Add(movie);

            return _cinemaDbContext.SaveChanges() > 0;
        }

        public List<Contracts.Movie>? GetAllMovies()
        {
            var movies = _cinemaDbContext.Set<Movie>()
                .Include(x => x.Directors)
                .Include(x => x.Genres)
                .Include(x => x.Countries)
                .ToList();

            if (movies.Count == 0) { return null; }

            return movies.Select(movie => new Contracts.Movie
            {
                MovieUid = movie.MovieUid,
                Title = movie.Title,
                ReleaseYear = movie.ReleaseYear,
                Duration = movie.Duration,
                Description = movie.Description,
                Image = movie.Image,
                Directors = movie.Directors.Select(x => x.FullName).ToList(),
                Countries = movie.Countries.Select(x => x.Name).ToList(),
                Genres = movie.Genres.Select(x => x.Name).ToList()
            }).ToList();
        }

        public Contracts.Movie? GetSingleMovie(Guid movieUid)
        {
            var movie = _cinemaDbContext.Set<Movie>()
                .Include(x => x.Directors)
                .Include(x => x.Genres)
                .Include(x => x.Countries)
                .SingleOrDefault(x => x.MovieUid == movieUid);

            if (movie == null) { return null; }

            return new Contracts.Movie
            {
                MovieUid = movie.MovieUid,
                Title = movie.Title,
                ReleaseYear = movie.ReleaseYear,
                Duration = movie.Duration,
                Description = movie.Description,
                Image = movie.Image,
                Directors = movie.Directors.Select(x => x.FullName).ToList(),
                Countries = movie.Countries.Select(x => x.Name).ToList(),
                Genres = movie.Genres.Select(x => x.Name).ToList()
            };
        }

        public List<Contracts.MovieInfo>? GetMoviesInfo()
        {
            var movies = _cinemaDbContext.Set<Movie>()
                .Include(x => x.Directors)
                .Include(x => x.Genres)
                .Include(x => x.Countries)
                .ToList();

            if (movies.Count == 0) { return null; }

            return movies.Select(movie => new Contracts.MovieInfo
            {
                Title = movie.Title,
                ReleaseYear = movie.ReleaseYear,
                Duration = movie.Duration,
                Description = movie.Description,
                Image = movie.Image,
                Directors = movie.Directors.Select(x => x.FullName).ToList(),
                Countries = movie.Countries.Select(x => x.Name).ToList(),
                Genres = movie.Genres.Select(x => x.Name).ToList()
            }).ToList();
        }

        public bool UpdateMovie(Guid movieUid, Contracts.MovieInfo movieInfo)
        {
            var movie = _cinemaDbContext.Set<Movie>()
                .Include(x => x.Directors)
                .Include(x => x.Countries)
                .Include(x => x.Genres)
                .SingleOrDefault(x => x.MovieUid == movieUid);

            if (movie == null) { return false; }

            movie.Title = movieInfo.Title;
            movie.ReleaseYear = movieInfo.ReleaseYear;
            movie.Duration = movieInfo.Duration;
            movie.Description = movieInfo.Description;

            movie.Directors.Clear();
            movie.Countries.Clear();
            movie.Genres.Clear();

            AddDirectors(movieInfo.Directors, movie);
            AddCountries(movieInfo.Countries, movie);
            AddGenres(movieInfo.Genres, movie);

            return _cinemaDbContext.SaveChanges() > 0;
        }

        public bool DeleteMovie(Guid movieUid)
        {
            var movie = _cinemaDbContext.Set<Movie>().SingleOrDefault(x => x.MovieUid == movieUid);

            if (movie == null) { return false; }

            _cinemaDbContext.Remove(movie);

            return _cinemaDbContext.SaveChanges() > 0;
        }

        private void AddDirectors(List<string> directorNames, Movie movie)
        {
            foreach (var directorName in directorNames)
            {
                var director = _cinemaDbContext.Set<Director>().SingleOrDefault(x => x.FullName == directorName);

                if (director == null)
                {
                    director = new Director
                    {
                        DirectorUid = Guid.NewGuid(),
                        FullName = directorName
                    };

                    _cinemaDbContext.Add(director);
                }

                movie.Directors.Add(director);
            }
        }

        private void AddCountries(List<string> countryNames, Movie movie)
        {
            foreach (var countryName in countryNames)
            {
                var country = _cinemaDbContext.Set<Country>().SingleOrDefault(x => x.Name == countryName);

                if (country == null)
                {
                    country = new Country
                    {
                        CountryUid = Guid.NewGuid(),
                        Name = countryName
                    };

                    _cinemaDbContext.Add(country);
                }

                movie.Countries.Add(country);
            }
        }

        private void AddGenres(List<string> genreNames, Movie movie)
        {
            foreach (var genreName in genreNames)
            {
                var genre = _cinemaDbContext.Set<Genre>().SingleOrDefault(x => x.Name == genreName);

                if (genre == null)
                {
                    genre = new Genre
                    {
                        GenreUid = Guid.NewGuid(),
                        Name = genreName
                    };

                    _cinemaDbContext.Add(genre);
                }

                movie.Genres.Add(genre);
            }
        }

        public bool CheckMovieExists(Guid movieUid)
        {
            var movie = _cinemaDbContext.Set<Movie>().SingleOrDefault(x => x.MovieUid == movieUid);

            if (movie == null) { return false; }

            return true;
        }

        public bool CheckMovieTitle(string movieTitle)
        {
            var movie = _cinemaDbContext.Set<Movie>().SingleOrDefault(x => x.Title == movieTitle);

            if (movie == null) { return false; }

            return true;
        }

        public bool CheckMovieInfo(Contracts.MovieInfo movieInfo)
        {
            var movie = _cinemaDbContext.Set<Movie>()
                .SingleOrDefault(x => x.Title == movieInfo.Title && 
                                x.ReleaseYear == movieInfo.ReleaseYear && 
                                x.Duration == movieInfo.Duration && 
                                x.Description == movieInfo.Description);

            if (movie == null) { return false; };

            return true;
        }
    }
}
