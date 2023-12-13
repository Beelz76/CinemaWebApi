namespace WebApi.Contracts
{
    public class Screening
    {
        public required Guid ScreeingUid { get; init; }

        public required string MovieTitle { get; init; }

        public required string MovieDuration { get; init; }

        public required string ScreeningStart { get; init; }

        public required string ScreeningEnd { get; init; }

        public required string HallName { get; init; }

        public required int Price { get; init; }
    }
}
