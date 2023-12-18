using DatabaseAccessLayer;
using DatabaseAccessLayer.Entities;
using System.Text.RegularExpressions;
using WebApi.Interface;

namespace WebApi.Services
{
    public class GenreService : IGenreService
    {
        private readonly CinemaDbContext _cinemaDbContext;

        public GenreService(CinemaDbContext cinemaDbContext)
        {
            _cinemaDbContext = cinemaDbContext;
        }

        public bool CreateGenre(string name)
        {
            var genre = new Genre
            {
                GenreUid = Guid.NewGuid(),
                Name = name
            };

            _cinemaDbContext.Add(genre);

            return _cinemaDbContext.SaveChanges() > 0;
        }

        public List<Contracts.Genre>? GetGenres()
        {
            var genres = _cinemaDbContext.Set<Genre>().ToList();

            if (genres.Count == 0) { return null; }

            return genres.Select(genre => new Contracts.Genre
            {
                GenreUid = genre.GenreUid,
                Name = genre.Name
            }).ToList();
        }

        public bool UpdateGenre(Guid genreUid, string name)
        {
            var genre = _cinemaDbContext.Set<Genre>().SingleOrDefault(x => x.GenreUid == genreUid);

            if (genre == null) { return false; }

            genre.Name = name;

            return _cinemaDbContext.SaveChanges() > 0;
        }

        public bool DeleteGenre(Guid genreUid)
        {
            var genre = _cinemaDbContext.Set<Genre>().SingleOrDefault(x => x.GenreUid == genreUid);

            if (genre == null) { return false; }

            _cinemaDbContext.Remove(genre);

            return _cinemaDbContext.SaveChanges() > 0;
        }

        public bool CheckGenreName(string name)
        {
            var genre = _cinemaDbContext.Set<Genre>().SingleOrDefault(x => x.Name == name);

            if (genre == null) { return false; }

            return true;
        }

        public bool IsGenreExists(Guid genreUid)
        {
            var genre = _cinemaDbContext.Set<Genre>().SingleOrDefault(x => x.GenreUid == genreUid);

            if (genre == null) { return false; }

            return true;
        }

        public bool CheckRegex(string name)
        {
            var regex = new Regex(@"^[a-zA-Zа-яА-Я][a-zA-Zа-яА-Я -]{1,}$");

            if (!regex.IsMatch(name))
            {
                return false;
            }

            return true;
        }
    }
}
