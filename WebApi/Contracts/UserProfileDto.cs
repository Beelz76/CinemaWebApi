namespace WebApi.Contracts
{
    public class UserProfileDto
    {
        public string FullName { get; init; }
        public string Login { get; init; }
        public string Password { get; init; }
        public string? Email { get; init; }
    }
}