namespace WebApi.Contracts
{
    public class SeatDto
    {
        public Guid SeatUid { get; init; }
        public string HallName { get; init; }
        public int Row { get; init; }
        public int Number { get; init; }
    }
}