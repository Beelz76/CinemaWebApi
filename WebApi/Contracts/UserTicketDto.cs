namespace WebApi.Contracts
{
    public class UserTicketDto
    {
        public Guid TicketUid { get; init; }    
        public string MovieTitle { get; init; }
        public string MovieDuration { get; init; }
        public string ScreeningStart { get; init; }
        public string ScreeningEnd { get; init; }
        public int Price { get; init; }
        public string HallName { get; init; }
        public int Row { get; init; }
        public int Number { get; init; }
    }
}