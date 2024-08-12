namespace WebApi.Contracts
{
    public class HallDto
    {
        public Guid HallUid { get; init; }
        public string Name { get; init; }
        public int Capacity { get; init; }
    }
}