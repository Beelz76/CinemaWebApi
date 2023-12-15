namespace WebApi.Contracts
{
    public class Movie
    {
        public required Guid MovieUid { get; init; }

        public required string Title { get; init; }

        public required int ReleaseYear { get; init; }

        public required int Duration { get; init; }

        public required string? Description { get; init; }

        public required List<string> Directors { get; init; }

        public required List<string> Countries { get; init; }

        public required List<string> Genres { get; init; }
    }
}
