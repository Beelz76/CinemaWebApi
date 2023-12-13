using DatabaseAccessLayer;
using DatabaseAccessLayer.Entities;
using System.Text.RegularExpressions;

namespace WebApi.Services
{
    public class CountryService
    {
        private readonly CinemaDbContext _cinemaDbContext;

        public CountryService(CinemaDbContext cinemaDbContext)
        {
            _cinemaDbContext = cinemaDbContext;
        }

        public bool CreateCountry(string name)
        {
            var country = new Country
            {
                CountryUid = Guid.NewGuid(),
                Name = name
            };

            _cinemaDbContext.Add(country);

            return _cinemaDbContext.SaveChanges() > 0;
        }

        public List<Contracts.Country>? GetCountries()
        {
            var countries = _cinemaDbContext.Set<Country>().ToList();

            if (countries.Count == 0) { return null; }

            return countries.Select(country => new Contracts.Country
            {
                CountryUid = country.CountryUid,
                Name = country.Name
            }).ToList();
        }

        public bool UpdateCountry(Guid countryUid, string name)
        {
            var country = _cinemaDbContext.Set<Country>().SingleOrDefault(x => x.CountryUid == countryUid);

            if (country == null) { return false; }

            country.Name = name;

            return _cinemaDbContext.SaveChanges() > 0;
        }

        public bool DeleteCountry(Guid countryUid)
        {
            var country = _cinemaDbContext.Set<Country>().SingleOrDefault(x => x.CountryUid == countryUid);

            if (country == null) { return false; }

            _cinemaDbContext.Remove(country);

            return _cinemaDbContext.SaveChanges() > 0;
        }

        public bool CheckCountryName(string name)
        {
            var country = _cinemaDbContext.Set<Country>().SingleOrDefault(x => x.Name == name);

            if (country == null) { return false; }

            return true;
        }

        public bool CheckCountryExists(Guid countryUid)
        {
            var country = _cinemaDbContext.Set<Country>().SingleOrDefault(x => x.CountryUid == countryUid);

            if (country == null) { return false; }

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
