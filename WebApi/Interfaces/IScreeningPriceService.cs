namespace WebApi.Interfaces
{
    public interface IScreeningPriceService
    {
        Task<bool> CreateScreeningPriceAsync(int price);
        Task<IReadOnlyList<Contracts.ScreeningPrice>> GetScreeningPricesAsync();
        Task<bool> UpdateScreeningPriceAsync(Guid screeningPriceUid, int price);
        Task<bool> DeleteScreeningPriceAsync(Guid screeningPriceUid);
        Task<bool> ScreeningPriceExistsAsync(int price);
    }
}