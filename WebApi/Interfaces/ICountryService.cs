namespace WebApi.Interface
{
    public interface ICountryService
    {
        Task<bool> CreateCountryAsync(string name);
        Task<List<Contracts.Country>> GetCountriesAsync();
        Task<bool> UpdateCountryAsync(Guid countryUid, string name);
        Task<bool> DeleteCountryAsync(Guid countryUid);
        Task<bool> CountryExistsAsync(string name);
        bool IsValidCountryName(string name);
    }
}