﻿using DatabaseAccessLayer;
using DatabaseAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using WebApi.Interfaces;

namespace WebApi.Services
{
    public class MovieService : IMovieService
    {
        private readonly CinemaDbContext _cinemaDbContext;

        public MovieService(CinemaDbContext cinemaDbContext, HttpClient httpClient)
        {
            _cinemaDbContext = cinemaDbContext;
        }

        public async Task<bool> CreateMovieAsync(Contracts.MovieInfo movieInfo)
        {
            var movie = new Movie
            {
                MovieUid = Guid.NewGuid(),
                Title = movieInfo.Title,
                ReleaseYear = movieInfo.ReleaseYear,
                Duration = int.Parse(movieInfo.Duration),
            };

            await AddDirectorsAsync(movieInfo.Directors, movie);
            await AddCountriesAsync(movieInfo.Countries, movie);
            await AddGenresAsync(movieInfo.Genres, movie);

            await _cinemaDbContext.AddAsync(movie);
            return await _cinemaDbContext.SaveChangesAsync() > 0;
        }

        public async Task<IReadOnlyList<Contracts.Movie>> GetAllMoviesAsync()
        {
            return await _cinemaDbContext.Set<Movie>()
                .Include(x => x.Directors)
                .Include(x => x.Genres)
                .Include(x => x.Countries)
                .Select(movie => new Contracts.Movie
                {
                    MovieUid = movie.MovieUid,
                    Title = movie.Title,
                    ReleaseYear = movie.ReleaseYear,
                    Duration = movie.Duration,
                    Directors = movie.Directors.Select(x => x.FullName).ToList(),
                    Countries = movie.Countries.Select(x => x.Name).ToList(),
                    Genres = movie.Genres.Select(x => x.Name).ToList()
                })
                .ToListAsync();
        }

        public async Task<Contracts.Movie> GetSingleMovieAsync(Guid movieUid)
        {
            var movie = await _cinemaDbContext.Set<Movie>()
                .Include(x => x.Directors)
                .Include(x => x.Genres)
                .Include(x => x.Countries)
                .Select(m => new Contracts.Movie
                {
                    MovieUid = m.MovieUid,
                    Title = m.Title,
                    ReleaseYear = m.ReleaseYear,
                    Duration = m.Duration,
                    Directors = m.Directors.Select(x => x.FullName).ToList(),
                    Countries = m.Countries.Select(x => x.Name).ToList(),
                    Genres = m.Genres.Select(x => x.Name).ToList()
                })
                .FirstOrDefaultAsync(x => x.MovieUid == movieUid);

            return movie;
        }

        public async Task<IReadOnlyList<Contracts.MovieInfo>> GetMoviesInfoAsync()
        {
            return await _cinemaDbContext.Set<Movie>()
                .Include(x => x.Directors)
                .Include(x => x.Genres)
                .Include(x => x.Countries)
                .Select(movie => new Contracts.MovieInfo
                {
                    movieUid = movie.MovieUid,
                    Title = movie.Title,
                    ReleaseYear = movie.ReleaseYear,
                    Duration = $"{movie.Duration / 60}ч {movie.Duration % 60}мин",
                    Directors = movie.Directors.Select(x => x.FullName).ToList(),
                    Countries = movie.Countries.Select(x => x.Name).ToList(),
                    Genres = movie.Genres.Select(x => x.Name).ToList()
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateMovieAsync(Guid movieUid, Contracts.MovieInfo movieInfo)
        {
            var movie = await _cinemaDbContext.Set<Movie>()
                .Include(x => x.Directors)
                .Include(x => x.Countries)
                .Include(x => x.Genres)
                .FirstOrDefaultAsync(x => x.MovieUid == movieUid);

            if (movie == null) { return false; }

            movie.Title = movieInfo.Title;
            movie.ReleaseYear = movieInfo.ReleaseYear;
            movie.Duration = int.Parse(movieInfo.Duration);

            movie.Directors.Clear();
            movie.Countries.Clear();
            movie.Genres.Clear();

            await AddDirectorsAsync(movieInfo.Directors, movie);
            await AddCountriesAsync(movieInfo.Countries, movie);
            await AddGenresAsync(movieInfo.Genres, movie);

            return await _cinemaDbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteMovieAsync(Guid movieUid)
        {
            var movie = await _cinemaDbContext.Set<Movie>().FirstOrDefaultAsync(x => x.MovieUid == movieUid);

            if (movie == null) { return false; }

            _cinemaDbContext.Remove(movie);
            return await _cinemaDbContext.SaveChangesAsync() > 0;
        }

        private async Task AddDirectorsAsync(List<string> directorNames, Movie movie)
        {
            foreach (var directorName in directorNames)
            {
                var director = await _cinemaDbContext.Set<Director>().FirstOrDefaultAsync(x => x.FullName == directorName);

                if (director == null)
                {
                    director = new Director
                    {
                        DirectorUid = Guid.NewGuid(),
                        FullName = directorName
                    };

                    await _cinemaDbContext.AddAsync(director);
                }

                movie.Directors.Add(director);
            }
        }

        private async Task AddCountriesAsync(List<string> countryNames, Movie movie)
        {
            foreach (var countryName in countryNames)
            {
                var country = await _cinemaDbContext.Set<Country>().FirstOrDefaultAsync(x => x.Name == countryName);

                if (country == null)
                {
                    country = new Country
                    {
                        CountryUid = Guid.NewGuid(),
                        Name = countryName
                    };

                    await _cinemaDbContext.AddAsync(country);
                }

                movie.Countries.Add(country);
            }
        }

        private async Task AddGenresAsync(List<string> genreNames, Movie movie)
        {
            foreach (var genreName in genreNames)
            {
                var genre = await _cinemaDbContext.Set<Genre>().FirstOrDefaultAsync(x => x.Name == genreName);

                if (genre == null)
                {
                    genre = new Genre
                    {
                        GenreUid = Guid.NewGuid(),
                        Name = genreName
                    };

                    await _cinemaDbContext.AddAsync(genre);
                }

                movie.Genres.Add(genre);
            }
        }

        public async Task<bool> MovieExistsAsync(Guid movieUid)
        {
            return await _cinemaDbContext.Set<Movie>().AnyAsync(x => x.MovieUid == movieUid);
        }

        public async Task<bool> MovieExistsByTitleAsync(string movieTitle)
        {
            var movie = await _cinemaDbContext.Set<Movie>().FirstOrDefaultAsync(x => x.Title == movieTitle);

            if (movie == null) { return false; }

            return true;
        }

        public async Task<bool> MovieExistsByInfoAsync(Contracts.MovieInfo movieInfo, Guid? movieUid = null)
        {
            var query = _cinemaDbContext.Set<Movie>()
                .Where(x => x.Title == movieInfo.Title &&
                            x.ReleaseYear == movieInfo.ReleaseYear &&
                            x.Duration == int.Parse(movieInfo.Duration));

            if (movieUid.HasValue)
            {
                query = query.Where(x => x.MovieUid != movieUid.Value);
            }

            var movie = await query.FirstOrDefaultAsync();

            if (movie == null) { return false; };

            return true;
        }

        public bool IsValidMovieTitle(string title)
        {
            return new Regex(@"^[a-zA-Zа-яА-Я][a-zA-Zа-яА-Я0-9. -]{1,}$").IsMatch(title);
        }

        public bool IsValidNamesInList(List<string> list)
        {
            var regex = new Regex(@"^[a-zA-Zа-яА-Я][a-zA-Zа-яА-Я -]{1,}$");

            foreach (var item in list)
            {
                if (!regex.IsMatch(item))
                {
                    return false;
                }
            }

            return true;
        }
    }
}