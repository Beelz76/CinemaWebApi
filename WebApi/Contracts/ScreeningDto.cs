namespace WebApi.Contracts
{
    public class ScreeningDto
    {
        public Guid ScreeningUid { get; init; }
        public string MovieTitle { get; init; }
        public string MovieDuration { get; init; }
        public string ScreeningStart { get; init; }
        public string ScreeningEnd { get; init; }
        public string HallName { get; init; }
        public int Price { get; init; }
    }
}