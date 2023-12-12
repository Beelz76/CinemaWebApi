namespace WebApi.Contracts
{
    public class Genre
    {
        public required Guid GenreUid { get; init; }

        public required string Name { get; init; }
    }
}
