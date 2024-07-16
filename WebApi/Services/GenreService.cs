using DatabaseAccessLayer;
using DatabaseAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> CreateGenreAsync(string name)
        {
            var genre = new Genre
            {
                GenreUid = Guid.NewGuid(),
                Name = name
            };

            await _cinemaDbContext.AddAsync(genre);
            return await _cinemaDbContext.SaveChangesAsync() > 0;
        }

        public async Task<List<Contracts.Genre>> GetGenresAsync()
        {
            return await _cinemaDbContext.Set<Genre>()
                .Select(genre => new Contracts.Genre
                {
                    GenreUid = genre.GenreUid,
                    Name = genre.Name
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateGenreAsync(Guid genreUid, string name)
        {
            var genre = await _cinemaDbContext.Set<Genre>().FirstOrDefaultAsync(x => x.GenreUid == genreUid);

            if (genre == null) { return false; }

            genre.Name = name;

            return await _cinemaDbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteGenreAsync(Guid genreUid)
        {
            var genre = await _cinemaDbContext.Set<Genre>().FirstOrDefaultAsync(x => x.GenreUid == genreUid);

            if (genre == null) { return false; }

            _cinemaDbContext.Remove(genre);
            return await _cinemaDbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> GenreExistsAsync(string name)
        {
            return await _cinemaDbContext.Set<Genre>().AnyAsync(x => x.Name == name);
        }

        public bool IsValidGenreName(string name)
        {
            return new Regex(@"^[a-zA-Zа-яА-Я][a-zA-Zа-яА-Я -]{1,}$").IsMatch(name);
        }
    }
}