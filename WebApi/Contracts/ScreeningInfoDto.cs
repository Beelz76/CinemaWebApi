namespace WebApi.Contracts
{
    public class ScreeningInfoDto
    {
        public string MovieTitle { get; init; }
        public string HallName { get; init; }
        public string ScreeningStart { get; init; }
        public int Price { get; init; }
    }
}