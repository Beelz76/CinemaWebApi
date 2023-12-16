namespace DatabaseAccessLayer.Entities
{
    public class Movie
    {
        public int MovieId { get; init; }

        public required Guid MovieUid { get; init; }

        public required string Title { get; set; }

        public required int ReleaseYear { get; set; }

        public required int Duration { get; set; }

        public ICollection<Director> Directors { get; set; } = new List<Director>();

        public ICollection<Country> Countries { get; set; } = new List<Country>();
        
        public ICollection<Genre> Genres { get; set; } = new List<Genre>();
        
        public ICollection<Screening> Screenings { get; set; }
    }
}
