namespace WebApi.Contracts
{
    public class UserLoginCredentials
    {
        public required string Login { get; init; }

        public required string Password { get; init; }
    }
}
