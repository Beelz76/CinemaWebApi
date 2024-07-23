namespace WebApi.Interface
{
    public interface IUserService
    {
        Task<Guid> RegisterAsync(Contracts.UserRegisterCredentials credentials);
        Task<Guid> LoginAsync(Contracts.UserLoginCredentials credentials);
        Task<IReadOnlyList<Contracts.User>> GetAllUsersAsync();
        Task<Contracts.User> GetSingleUserAsync(Guid userUid);
        Task<Contracts.UserInfo> GetUserInfoAsync(Guid userUid);
        Task<bool> UpdateUserAsync(Guid userUid, Contracts.UserUpdate userUpdate);
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