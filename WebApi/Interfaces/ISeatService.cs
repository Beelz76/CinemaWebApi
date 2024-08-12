using WebApi.Contracts;

namespace WebApi.Interfaces
{
    public interface ISeatService
    {
        Task<bool> CreateSeatAsync(string hallName, int row, int number);
        Task<IReadOnlyList<SeatDto>> GetAllSeatsAsync();
        Task<IReadOnlyList<HallSeatDto>> GetHallSeatsAsync(string hallName);
        Task<IReadOnlyList<ScreeningSeatDto>> GetScreeningSeatsAsync(Guid screeningUid);
        Task<bool> UpdateSeatAsync(Guid seatUid, int row, int number);
        Task<bool> DeleteSeatAsync(Guid seatUid);
        Task<bool> SeatExistsAsync(string hallName, int row, int number);
        Task<bool> ScreeningSeatExistsAsync(Guid screeningUid, Guid seatUid);
        Task<bool> IsSeatTakenAsync(Guid screeningUid, Guid seatUid);
    }
}