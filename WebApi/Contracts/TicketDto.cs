namespace WebApi.Contracts
{
    public class TicketDto
    {
        public Guid TicketUid { get; init; }
        public string UserFullName { get; init; }
        public string MovieTitle { get; init; }
        public int MovieDuration { get; init; }
        public string ScreeningStart { get; init; }
        public string ScreeningEnd { get; init; }
        public int Price { get; init; }
        public string HallName { get; init; }
        public int Row { get; init; }
        public int Number { get; init; }
    }
}