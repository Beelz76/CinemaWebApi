namespace WebApi.Interface
{
    public interface IScreeningPriceService
    {
        bool CreateScreeningPrice(int price);
        List<Contracts.ScreeningPrice>? GetScreeningPrices();
        bool UpdateScreeningPrice(Guid screeningPriceUid, int price);
        bool DeleteScreeningPrice(Guid screeningPriceUid);
        bool CheckScreeningPrice(int price);
        bool IsScreeningPriceExists(Guid screeningPriceUid);
    }
}