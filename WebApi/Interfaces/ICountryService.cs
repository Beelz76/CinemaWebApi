namespace WebApi.Interface
{
    public interface ICountryService
    {
        bool CreateCountry(string name);
        List<Contracts.Country>? GetCountries();
        bool UpdateCountry(Guid countryUid, string name);
        bool DeleteCountry(Guid countryUid);
        bool CheckCountryName(string name);
        bool IsCountryExists(Guid countryUid);
        bool CheckRegex(string name);
    }
}