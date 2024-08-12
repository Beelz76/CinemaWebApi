namespace WebApi.Contracts
{
    public class RegisterUserDto
    {
        public string FullName { get; init; }
        public string Login { get; init; }
        public string Password { get; init; }
    }
}