namespace WebApi.Contracts
{
    public class HallSeat
    {
        public required Guid SeatUid { get; init; }

        public required int Row { get; init; }

        public required int Number { get; init; }
    }
}