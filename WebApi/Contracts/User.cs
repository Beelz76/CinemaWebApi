namespace WebApi.Contracts
{
    public class User
    {
        public Guid UserUid { get; init; }

        public required string FullName { get; init; }

        public required string Login { get; init; }

        public required string Password { get; init; }

        public required string? Email { get; init; }

        public bool IsAdmin { get; init; } = false;
    }
}
