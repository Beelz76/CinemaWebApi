﻿namespace WebApi.Contracts
{
    public class UserTicket
    {
        public required string MovieTitle { get; init; }

        public required int MovieDuration { get; init; }

        public required string ScreeningStart { get; init; }

        public required string ScreeningEnd { get; init; }

        public required int Price { get; init; }

        public required string HallName { get; init; }

        public required int Row { get; init; }

        public required int Number { get; init; }
    }
}