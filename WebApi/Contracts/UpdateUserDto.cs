namespace WebApi.Contracts
{
    public class UpdateUserDto
    {
        public string FullName { get; init; }
        public string Login { get; init; }
        public string Password { get; init; }
        public string ConfirmedPassword { get; init; }
        public string? Email { get; init; } 
    }
}