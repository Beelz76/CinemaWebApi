namespace DatabaseAccessLayer.Entities
{
    public class Director
    {
        public int DirectorId { get; init; }

        public required Guid DirectorUid { get; init; }

        public required string FullName { get; set; }

        public ICollection<Movie> Movies { get; set; }
    }
}
