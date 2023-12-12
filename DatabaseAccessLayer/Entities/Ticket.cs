namespace DatabaseAccessLayer.Entities
{
    public class Ticket
    {
        public int TicketId { get; init; }

        public required Guid TicketUid { get; init; }

        public required User User { get; init; }

        public required Screening Screening { get; init; }

        public required Seat Seat { get; init; }
    }
}
