namespace WebApi.Contracts
{
    public class Director
    {
        public required Guid DirectorUid { get; init; }

        public required string FullName { get; init; }
    }
}
