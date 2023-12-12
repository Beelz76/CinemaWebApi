namespace WebApi.Contracts
{
    public class ScreeningUpdate
    {
        public required string MovieTitle { get; init; }

        public required string HallName { get; init; }

        public required DateTime ScreeningStart { get; init; }

        public required int Price { get; init; }
    }
}
