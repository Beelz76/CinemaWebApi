namespace WebApi.Contracts
{
    public class UserDto
    {
        public Guid UserUid { get; init; }
        public string FullName { get; init; }
        public string Login { get; init; }
        public string Password { get; init; }
        public string? Email { get; init; }
        public bool IsAdmin { get; init; }
    }
}