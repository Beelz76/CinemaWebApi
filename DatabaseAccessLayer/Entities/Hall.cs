namespace DatabaseAccessLayer.Entities
{
    public class Hall
    {
        public int HallId { get; init; }

        public required Guid HallUid { get; init; }

        public required string Name { get; set; }

        public required int Capacity { get; set; }

        public ICollection<Screening> Screenings { get; set; }

        public ICollection<Seat> Seats { get; set; }
    }
}
