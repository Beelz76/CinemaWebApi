namespace WebApi.Interface
{
    public interface IScreeningService
    {
        Task<bool> CreateScreeningAsync(Contracts.ScreeningInfo screeningInfo);
        Task<List<Contracts.Screening>> GetAllScreeningsAsync();
        Task<List<Contracts.MovieScreening>> GetMovieScreeningsAsync(Guid movieUid);
        Task<List<Contracts.Screening>> GetHallScreeningsAsync(string hallName);
        Task<bool> UpdateScreeningAsync(Guid screeningUid, Contracts.ScreeningInfo screeningInfo);
        Task<bool> DeleteScreeningAsync(Guid screeningUid);
        Task<bool> IsValidScreningTimeAsync(string movieTitle, string hallName, string screeningStart);     
        Task<bool> ScreeningExistsAsync(Guid screeningUid);
    }
}