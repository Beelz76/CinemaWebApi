namespace WebApi.Contracts
{
    public class ScreeningInfo
    {
        public required string HallName { get; init; }

        public required DateTime ScreeningStart { get; init; }

        public required DateTime ScreeningEnd { get; init; }

        public required int Price { get; init; }
    }
}
