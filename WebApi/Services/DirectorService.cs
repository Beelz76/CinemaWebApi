using DatabaseAccessLayer;
using DatabaseAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using WebApi.Interface;

namespace WebApi.Services
{
    public class DirectorService : IDirectorService
    {
        private readonly CinemaDbContext _cinemaDbContext;

        public DirectorService(CinemaDbContext cinemaDbContext)
        {
            _cinemaDbContext = cinemaDbContext;
        }

        public async Task<bool> CreateDirectorAsync(string fullName)
        {
            var director = new Director
            {
                DirectorUid = Guid.NewGuid(),
                FullName = fullName
            };

            await _cinemaDbContext.AddAsync(director);
            return await _cinemaDbContext.SaveChangesAsync() > 0;
        }

        public async Task<List<Contracts.Director>> GetDirectorsAsync()
        {
            var directors = await _cinemaDbContext.Set<Director>().ToListAsync();

            if (directors.Count == 0) { return new List<Contracts.Director>(); }

            return directors.Select(director => new Contracts.Director
            {
                DirectorUid = director.DirectorUid,
                FullName = director.FullName
            }).ToList();
        }

        public async Task<bool> UpdateDirectorAsync(Guid directorUid, string fullName)
        {
            var director = await _cinemaDbContext.Set<Director>().FirstOrDefaultAsync(x => x.DirectorUid == directorUid);

            if (director == null) { return false; }

            director.FullName = fullName;

            return await _cinemaDbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteDirectorAsync(Guid directorUid)
        {
            var director = await _cinemaDbContext.Set<Director>().FirstOrDefaultAsync(x => x.DirectorUid == directorUid);

            if (director == null) { return false; }

            _cinemaDbContext.Remove(director);
            return await _cinemaDbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DirectorExistsAsync(string fullName)
        {
            return await _cinemaDbContext.Set<Director>().AnyAsync(x => x.FullName == fullName);
        }

        public bool IsValidDirectorName(string name)
        {
            return new Regex(@"^[a-zA-Zа-яА-Я][a-zA-Zа-яА-Я -]{1,}$").IsMatch(name);
        }
    }
}