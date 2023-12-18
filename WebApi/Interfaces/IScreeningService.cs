namespace WebApi.Interface
{
    public interface IScreeningService
    {
        bool CreateScreening(Contracts.ScreeningInfo screeningInfo);
        List<Contracts.Screening>? GetAllScreenings();
        List<Contracts.MovieScreening>? GetMovieScreenings(Guid movieUid);
        List<Contracts.Screening>? GetHallScreenings(string hallName);
        bool UpdateScreening(Guid screeningUid, Contracts.ScreeningInfo screeningInfo);
        bool DeleteScreening(Guid screeningUid);
        bool CheckScreeningInfo(string movieTitle, string hallName, string screeningStart);
        bool CheckScreeningInfo(string movieTitle, string hallName, string screeningStart, Guid screeningUid);
        bool IsScreeningExists(Guid screeningUid);
    }
}