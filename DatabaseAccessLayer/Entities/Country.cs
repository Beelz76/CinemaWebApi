namespace DatabaseAccessLayer.Entities
{
    public class Country
    {
        public int CountryId { get; init; }

        public required Guid CountryUid { get; init; }

        public required string Name { get; set; }

        public ICollection<Movie> Movies { get; set; }
    }
}
