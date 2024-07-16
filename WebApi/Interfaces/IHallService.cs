namespace WebApi.Interface
{
    public interface IHallService
    {
        Task<bool> CreateHallAsync(string name);
        Task<List<Contracts.Hall>> GetHallsAsync();
        Task<bool> UpdateHallAsync(Guid hallUid, string name);
        Task<bool> DeleteHallAsync(Guid hallUid);
        Task<bool> HallExistsAsync(string name);
        bool IsValidHallName(string name);
    }
}