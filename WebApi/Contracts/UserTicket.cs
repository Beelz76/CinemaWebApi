namespace WebApi.Contracts
{
    public class UserTicket
    {
        public required string MovieTitle { get; init; }

        public required int MovieDuration { get; init; }

        public required DateTime ScreeningStart { get; init; }

        public required DateTime ScreeningEnd { get; init; }

        public required int Price { get; init; }

        public required string HallName { get; init; }

        public required int Row { get; init; }

        public required int Number { get; init; }

        //public required ScreeningInfo ScreeningInfo { get; init; }
        //public required SeatInfo SeatInfo { get; init; }
    }
}