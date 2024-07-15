namespace WebApi.Interface
{
    public interface IScreeningPriceService
    {
        Task<bool> CreateScreeningPriceAsync(int price);
        Task<List<Contracts.ScreeningPrice>> GetScreeningPricesAsync();
        Task<bool> UpdateScreeningPriceAsync(Guid screeningPriceUid, int price);
        Task<bool> DeleteScreeningPriceAsync(Guid screeningPriceUid);
        Task<bool> ScreeningPriceExistsAsync(int price);
    }
}