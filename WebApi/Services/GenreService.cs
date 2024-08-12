using DatabaseAccessLayer;
using DatabaseAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using WebApi.Contracts;
using WebApi.Interfaces;

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

        public async Task<IReadOnlyList<GenreDto>> GetGenresAsync()
        {
            return await _cinemaDbContext.Set<Genre>()
                .Select(genre => new GenreDto
                {
                    GenreUid = genre.GenreUid,
                    Name = genre.Name
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateGenreAsync(Guid genreUid, string name)
        {
            var totalRows = await _cinemaDbContext.Set<Genre>()
                .Where(x => x.GenreUid == genreUid)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(p => p.Name, p => name));
            
            return totalRows > 0;
        }

        public async Task<bool> DeleteGenreAsync(Guid genreUid)
        {
            var totalRows = await _cinemaDbContext.Set<Genre>()
                .Where(x => x.GenreUid == genreUid)
                .ExecuteDeleteAsync();
            
            return totalRows > 0;
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