using DatabaseAccessLayer;
using DatabaseAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using WebApi.Interface;

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

        public async Task<List<Contracts.Country>> GetCountriesAsync()
        {
            var countries = await _cinemaDbContext.Set<Country>()
                .Select(country => new Contracts.Country
                {
                    CountryUid = country.CountryUid,
                    Name = country.Name
                })
                .ToListAsync();

            if (countries.Count == 0) { return new List<Contracts.Country>(); }

            return countries;
        }

        public async Task<bool> UpdateCountryAsync(Guid countryUid, string name)
        {
            var country = await _cinemaDbContext.Set<Country>().FirstOrDefaultAsync(x => x.CountryUid == countryUid);

            if (country == null) { return false; }

            country.Name = name;

            return await _cinemaDbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteCountryAsync(Guid countryUid)
        {
            var country = await _cinemaDbContext.Set<Country>().FirstOrDefaultAsync(x => x.CountryUid == countryUid);

            if (country == null) { return false; }

            _cinemaDbContext.Remove(country);
            return await _cinemaDbContext.SaveChangesAsync() > 0;
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