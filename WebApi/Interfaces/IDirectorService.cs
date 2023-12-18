namespace WebApi.Interface
{
    public interface IDirectorService
    {
        bool CreateDirector(string fullName);
        List<Contracts.Director>? GetDirectors();
        bool UpdateDirector(Guid directorUid, string fullName);
        bool DeleteDirector(Guid directorUid);
        bool IsDirectorExists(Guid directorUid);
        bool CheckRegex(string name);
    }
}