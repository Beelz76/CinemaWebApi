using DatabaseAccessLayer;
using DatabaseAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using WebApi.Interfaces;

namespace WebApi.Services
{
    public class CountryService : ICountryService
    {
        private readonly CinemaDbContext _cinemaDbContext;

        public CountryService(CinemaDbContext cinemaDbContext)
        {
            _cinemaDbContext = cinemaDbContext;
        }

        public async Task<bool> CreateCountryAsync(string name)
        {
            var country = new Country
            {
                CountryUid = Guid.NewGuid(),
                Name = name
            };

            await _cinemaDbContext.AddAsync(country);
            return await _cinemaDbContext.SaveChangesAsync() > 0;
        }

        public async Task<IReadOnlyList<Contracts.Country>> GetCountriesAsync()
        {
            return await _cinemaDbContext.Set<Country>()
                .Select(c => new Contracts.Country
                {
                    CountryUid = c.CountryUid,
                    Name = c.Name
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateCountryAsync(Guid countryUid, string name)
        { 
            var totalRows = await _cinemaDbContext.Set<Country>()
                .Where(x => x.CountryUid == countryUid)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(p => p.Name, p => name));
            
            return totalRows > 0;
        }

        public async Task<bool> DeleteCountryAsync(Guid countryUid)
        {
            var totalRows = await _cinemaDbContext.Set<Country>()
                .Where(x => x.CountryUid == countryUid)
                .ExecuteDeleteAsync();
            
            return totalRows > 0;
        }

        public async Task<bool> CountryExistsAsync(string name)
        {
            return await _cinemaDbContext.Set<Country>().AnyAsync(x => x.Name == name);
        }

        public bool IsValidCountryName(string name)
        {
            return new Regex(@"^[a-zA-Zа-яА-Я][a-zA-Zа-яА-Я -]{1,}$").IsMatch(name);     
        }
    }
}