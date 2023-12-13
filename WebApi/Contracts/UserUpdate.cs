namespace WebApi.Contracts
{
    public class UserUpdate
    {
        public required string FullName { get; init; }

        public required string Login { get; init; }

        public required string Password { get; init; }

        public required string ConfirmedPassword { get; init; }

        public required string Email { get; init; } 
    }
}
