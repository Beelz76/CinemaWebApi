namespace WebApi.Contracts
{
    public class MovieInfo
    {
        public Guid movieUid { get; init; }

        public required string Title { get; init; }

        public required int ReleaseYear { get; init; }

        public required string Duration { get; init; }

        public required List<string> Directors { get; init; }

        public required List<string> Countries { get; init; }

        public required List<string> Genres { get; init; }
    }
}
