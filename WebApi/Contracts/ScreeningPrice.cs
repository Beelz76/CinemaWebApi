namespace WebApi.Contracts
{
    public class ScreeningPrice
    {
        public required Guid ScreeningPriceUid { get; init; }

        public required int Price { get; init; }
    }
}