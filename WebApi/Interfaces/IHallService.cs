namespace WebApi.Interface
{
    public interface IHallService
    {
        bool CreateHall(string name);
        List<Contracts.Hall>? GetHalls();
        bool UpdateHall(Guid hallUid, string name);
        bool DeleteHall(Guid hallUid);
        bool CheckHallName(string name);
        bool IsHallExists(Guid hallUid);
        bool CheckRegex(string name);
    }
}