using DatabaseAccessLayer;
using DatabaseAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using WebApi.Interfaces;

namespace WebApi.Services
{
    public class HallService : IHallService
    {
        private readonly CinemaDbContext _cinemaDbContext;

        public HallService(CinemaDbContext cinemaDbContext)
        {
            _cinemaDbContext = cinemaDbContext;
        }

        public async Task<bool> CreateHallAsync(string name)
        {
            var hall = new Hall
            {
                HallUid = Guid.NewGuid(),
                Name = name,
                Capacity = 0,
            };

            await _cinemaDbContext.AddAsync(hall);
            return await _cinemaDbContext.SaveChangesAsync() > 0;
        }

        public async Task<IReadOnlyList<Contracts.Hall>> GetHallsAsync()
        {
            return await _cinemaDbContext.Set<Hall>()
                .Select(hall => new Contracts.Hall
                {
                    HallUid = hall.HallUid,
                    Name = hall.Name,
                    Capacity = hall.Capacity
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateHallAsync(Guid hallUid, string name)
        {
            var totalRows = await _cinemaDbContext.Set<Hall>()
                .Where(x => x.HallUid == hallUid)   
                .ExecuteUpdateAsync(x => x
                    .SetProperty(p => p.Name, p => name));
            
            return totalRows > 0;
        }

        public async Task<bool> DeleteHallAsync(Guid hallUid)
        {
            var totalRows = await _cinemaDbContext.Set<Hall>()
                .Where(x => x.HallUid == hallUid)
                .ExecuteDeleteAsync();

            return totalRows > 0;
        }

        public async Task<bool> HallExistsAsync(string name)
        {
            return await _cinemaDbContext.Set<Hall>().AnyAsync(x => x.Name == name);
        }

        public bool IsValidHallName(string name)
        {
            return new Regex(@"^[a-zA-Zа-яА-Я0-9][a-zA-Zа-яА-Я -]{1,}$").IsMatch(name);
        }
    }
}