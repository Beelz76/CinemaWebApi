namespace WebApi.Interface
{
    public interface IUserService
    {
        Guid Register(Contracts.UserRegisterCredentials credentials);
        Guid? Login(Contracts.UserLoginCredentials credentials);
        List<Contracts.User>? GetAllUsers();
        Contracts.User? GetSingleUser(Guid userUid);
        Contracts.UserInfo? GetUserInfo(Guid userUid);
        bool UpdateUser(Guid userUid, Contracts.UserUpdate userUpdate);
        bool UpdateUserAdminStatus(Guid userUid);
        bool DeleteUser(Guid userUid);
        bool CheckLogin(string login);
        string? GetLogin(Guid userUid);
        bool IsAdmin(Guid userUid);
        bool IsUserExists(Guid? userUid);
        bool CheckLoginRegex(string login);
        bool CheckEmailRegex(string email);
    }
}