namespace DatabaseAccessLayer.Entities
{
    public class Seat
    {
        public int SeatId { get; init; }

        public required Guid SeatUid { get; init; }

        public required Hall Hall { get; set; }

        public required int Row { get; set; }

        public required int Number { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
    }
}
