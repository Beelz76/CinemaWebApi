using DatabaseAccessLayer;
using DatabaseAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using WebApi.Contracts;
using WebApi.Interfaces;

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

        public async Task<IReadOnlyList<DirectorDto>> GetDirectorsAsync()
        {
            return await _cinemaDbContext.Set<Director>()
                .Select(director => new DirectorDto
                {
                    DirectorUid = director.DirectorUid,
                    FullName = director.FullName
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateDirectorAsync(Guid directorUid, string fullName)
        {
            var totalRows = await _cinemaDbContext.Set<Director>()
                .Where(x => x.DirectorUid == directorUid)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(p => p.FullName, p => fullName));

            return totalRows > 0;
        }

        public async Task<bool> DeleteDirectorAsync(Guid directorUid)
        {
            var totalRows = await _cinemaDbContext.Set<Director>()
                .Where(x => x.DirectorUid == directorUid)
                .ExecuteDeleteAsync();

            return totalRows > 0;
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