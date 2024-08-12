using WebApi.Contracts;

namespace WebApi.Interfaces
{
    public interface ICountryService
    {
        Task<bool> CreateCountryAsync(string name);
        Task<IReadOnlyList<CountryDto>> GetCountriesAsync();
        Task<bool> UpdateCountryAsync(Guid countryUid, string name);
        Task<bool> DeleteCountryAsync(Guid countryUid);
        Task<bool> CountryExistsAsync(string name);
        bool IsValidCountryName(string name);
    }
}