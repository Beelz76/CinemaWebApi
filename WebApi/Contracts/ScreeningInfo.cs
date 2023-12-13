namespace WebApi.Contracts
{
    public class ScreeningInfo
    {
        public required string MovieTitle { get; init; }

        public required string HallName { get; init; }

        public required string ScreeningStart { get; init; }

        public required int Price { get; init; }
    }
}
