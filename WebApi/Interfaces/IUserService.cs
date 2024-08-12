using WebApi.Contracts;

namespace WebApi.Interfaces
{
    public interface IUserService
    {
        Task<Guid> RegisterAsync(RegisterUserDto credentials);
        Task<Guid> LoginAsync(LoginUserDto credentials);
        Task<IReadOnlyList<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetSingleUserAsync(Guid userUid);
        Task<UserProfileDto> GetUserInfoAsync(Guid userUid);
        Task<bool> UpdateUserAsync(Guid userUid, UpdateUserDto updateUserDto);
        Task<bool> UpdateUserAdminStatusAsync(Guid userUid);
        Task<bool> DeleteUserAsync(Guid userUid);
        Task<bool> LoginExistsAsync(string login);
        Task<string> GetUserLoginAsync(Guid userUid);
        Task<bool> IsAdminAsync(Guid userUid);
        Task<bool> UserExistsAsync(Guid userUid);
        bool IsValidLogin(string login);
        bool IsValidEmail(string email);
    }
}