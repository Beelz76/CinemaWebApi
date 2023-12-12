namespace WebApi.Contracts
{
    public class MovieScreening
    {
        public required string MovieTitle { get; init; }

        public required int MovieDuration { get; init; }

        public required string HallName { get; init; }

        public required DateTime ScreeningStart { get; init; }

        public required DateTime ScreeningEnd { get; init; }

        public required int Price { get; init; }

        //public required List<ScreeningInfo> ScreeningInfo { get; init; }
    }
}