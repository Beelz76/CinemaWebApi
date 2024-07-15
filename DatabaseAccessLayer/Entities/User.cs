namespace DatabaseAccessLayer.Entities
{
    public class User
    {
        public int UserId { get; init; }

        public required Guid UserUid { get; init; }

        public required string FullName { get; set; }

        public required string Login { get; set; }

        public required string Password { get; set; }

        public string? Email { get; set; }

        public required bool IsAdmin { get; set; } = false;

        public ICollection<Ticket> Tickets { get; set; }
    }
}
