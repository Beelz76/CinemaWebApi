namespace WebApi.Interface
{
    public interface IScreeningService
    {
        Task<bool> CreateScreeningAsync(Contracts.ScreeningInfo screeningInfo);
        Task<IReadOnlyList<Contracts.Screening>> GetAllScreeningsAsync();
        Task<IReadOnlyList<Contracts.MovieScreening>> GetMovieScreeningsAsync(Guid movieUid);
        Task<IReadOnlyList<Contracts.Screening>> GetHallScreeningsAsync(string hallName);
        Task<bool> UpdateScreeningAsync(Guid screeningUid, Contracts.ScreeningInfo screeningInfo);
        Task<bool> DeleteScreeningAsync(Guid screeningUid);
        Task<bool> IsValidScreningTimeAsync(string movieTitle, string hallName, string screeningStart);     
        Task<bool> ScreeningExistsAsync(Guid screeningUid);
    }
}