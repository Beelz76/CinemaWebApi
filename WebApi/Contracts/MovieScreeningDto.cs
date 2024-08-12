namespace WebApi.Contracts
{
    public class MovieScreeningDto
    {
        public Guid ScreeningUid { get; init; }
        public string ScreeningStart { get; init; }
        public string ScreeningEnd { get; init; }
        public string HallName { get; init; }
        public int Price { get; init; }
    }
}