namespace WebApi.Contracts
{
    public class ScreeningSeat
    {
        public required Guid SeatUid { get; init; }

        public required int Row { get; init; }

        public required int Number { get; init; }

        public required string Status { get; init; }
    }
}