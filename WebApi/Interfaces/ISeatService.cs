namespace WebApi.Interface
{
    public interface ISeatService
    {
        Task<bool> CreateSeatAsync(string hallName, int row, int number);
        Task<List<Contracts.Seat>> GetAllSeatsAsync();
        Task<List<Contracts.HallSeat>> GetHallSeatsAsync(string hallName);
        Task<List<Contracts.ScreeningSeat>> GetScreeningSeatsAsync(Guid screeningUid);
        Task<bool> UpdateSeatAsync(Guid seatUid, int row, int number);
        Task<bool> DeleteSeatAsync(Guid seatUid);
        Task<bool> SeatExistsAsync(string hallName, int row, int number);
    }
}