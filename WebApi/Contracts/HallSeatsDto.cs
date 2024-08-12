namespace WebApi.Contracts
{
    public class HallSeatDto
    {
        public Guid SeatUid { get; init; }
        public string HallName { get; init; }
        public int Row { get; init; }
        public int Number { get; init; }
    }
}