namespace WebApi.Contracts
{
    public class Hall
    {
        public required Guid HallUid { get; init; }

        public required string Name { get; init; }

        public required int Capacity { get; set; }
    }
}