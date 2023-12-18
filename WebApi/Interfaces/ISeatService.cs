namespace WebApi.Interface
{
    public interface ISeatService
    {
        bool CreateSeat(string hallName, int row, int number);
        List<Contracts.Seat>? GetAllSeats();
        List<Contracts.HallSeat>? GetHallSeats(string hallName);
        List<Contracts.ScreeningSeat>? GetScreeningSeats(Guid screeningUid);
        bool UpdateSeat(Guid seatUid, int row, int number);
        bool DeleteSeat(Guid seatUid);
        bool IsSeatExists(Guid seatUid);
        bool CheckSeat(string hallName, int row, int number);
    }
}