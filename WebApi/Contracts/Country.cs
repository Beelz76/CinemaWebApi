namespace WebApi.Contracts
{
    public class Country
    {
        public required Guid CountryUid { get; init; }

        public required string Name { get; init; }
    }
}
