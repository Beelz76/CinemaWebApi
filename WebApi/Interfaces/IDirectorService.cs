﻿namespace WebApi.Interface
{
    public interface IDirectorService
    {
        Task<bool> CreateDirectorAsync(string fullName);
        Task<List<Contracts.Director>> GetDirectorsAsync();
        Task<bool> UpdateDirectorAsync(Guid directorUid, string fullName);
        Task<bool> DeleteDirectorAsync(Guid directorUid);
        Task<bool> DirectorExistsAsync(string fullName);
        bool IsValidDirectorName(string name);
    }
}