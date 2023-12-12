namespace WebApi.Contracts
{
    public class UserRegisterCredentials
    {
        public required string FullName { get; init; }

        public required string Login { get; init; }

        public required string Password { get; init; }
    }
}
