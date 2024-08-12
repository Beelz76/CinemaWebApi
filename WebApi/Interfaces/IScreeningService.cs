using WebApi.Contracts;

namespace WebApi.Interfaces
{
    public interface IScreeningService
    {
        Task<bool> CreateScreeningAsync(ScreeningInfoDto screeningInfoDto);
        Task<IReadOnlyList<ScreeningDto>> GetAllScreeningsAsync();
        Task<IReadOnlyList<MovieScreeningDto>> GetMovieScreeningsAsync(Guid movieUid);
        Task<IReadOnlyList<ScreeningDto>> GetHallScreeningsAsync(string hallName);
        Task<bool> UpdateScreeningAsync(Guid screeningUid, ScreeningInfoDto screeningInfoDto);
        Task<bool> DeleteScreeningAsync(Guid screeningUid);
        Task<bool> IsValidScreeningTimeAsync(string movieTitle, string hallName, string screeningStart);     
        Task<bool> ScreeningExistsAsync(Guid screeningUid);
    }
}