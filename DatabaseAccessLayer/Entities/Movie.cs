namespace DatabaseAccessLayer.Entities
{
    public class Movie
    {
        public int MovieId { get; init; }

        public required Guid MovieUid { get; init; }

        public required string Title { get; set; }

        public required int ReleaseYear { get; set; }

        public required int Duration { get; set; }

        public string? Description { get; set; }

        public byte[]? Image { get; set; }

        public ICollection<Director> Directors { get; set; }

        public ICollection<Country> Countries { get; set; }
        
        public ICollection<Genre> Genres { get; set; }
        
        public ICollection<Screening> Screenings { get; set; }
    }
}
