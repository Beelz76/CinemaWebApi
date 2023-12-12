namespace DatabaseAccessLayer.Entities
{
    public class Genre
    {
        public int GenreId { get; init; }

        public required Guid GenreUid { get; init; }

        public required string Name { get; set; }

        public ICollection<Movie> Movies { get; set; }
    }
}
