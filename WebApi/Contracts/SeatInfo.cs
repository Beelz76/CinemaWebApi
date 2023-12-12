namespace WebApi.Contracts
{
    public class SeatInfo
    {
        public required int Row { get; init; }

        public required int Number { get; init; }

        public required string Status { get; init; }
    }
}