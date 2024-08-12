namespace WebApi.Contracts
{
    public class MovieInfoDto
    {
        public Guid MovieUid { get; init; }
        public string Title { get; init; }
        public int ReleaseYear { get; init; }
        public string Duration { get; init; }
        public List<string> Directors { get; init; }
        public List<string> Countries { get; init; }
        public List<string> Genres { get; init; }
    }
}