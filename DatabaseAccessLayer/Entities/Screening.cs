namespace DatabaseAccessLayer.Entities
{
    public class Screening
    {
        public int ScreeningId { get; init; }

        public required Guid ScreeningUid { get; init; }

        public required Movie Movie { get; set; }

        public required Hall Hall { get; set; }

        public required DateTime ScreeningStart { get; set; }

        public required DateTime ScreeningEnd { get; set; }

        public required ScreeningPrice ScreeningPrice { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
    }
}
