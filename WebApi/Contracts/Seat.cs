namespace WebApi.Contracts
{
    public class Seat
    {
        public required Guid SeatUid { get; init; }

        public required string HallName { get; init; }

        public required int Row { get; init; }

        public required int Number { get; init; }
    }
}