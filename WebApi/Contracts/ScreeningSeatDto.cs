namespace WebApi.Contracts
{
    public class ScreeningSeatDto
    {
        public Guid SeatUid { get; init; }
        public int Row { get; init; }
        public int Number { get; init; }
        public string Status { get; init; }
    }
}