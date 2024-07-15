namespace WebApi.Contracts
{
    public class MovieInfoo
    {
        public required string Title { get; init; }

        public required string Year { get; init; }

        public required string Genre { get; init; }

        public required string Director { get; init; }

        public required string Plot { get; init; }
    }
}
