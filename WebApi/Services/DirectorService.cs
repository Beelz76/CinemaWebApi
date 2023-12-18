using DatabaseAccessLayer;
using DatabaseAccessLayer.Entities;
using System.Text.RegularExpressions;
using WebApi.Interface;

namespace WebApi.Services
{
    public class DirectorService : IDirectorService
    {
        private readonly CinemaDbContext _cinemaDbContext;

        public DirectorService(CinemaDbContext cinemaDbContext)
        {
            _cinemaDbContext = cinemaDbContext;
        }

        public bool CreateDirector(string fullName)
        {
            var director = new Director
            {
                DirectorUid = Guid.NewGuid(),
                FullName = fullName
            };

            _cinemaDbContext.Add(director);

            return _cinemaDbContext.SaveChanges() > 0;
        }

        public List<Contracts.Director>? GetDirectors()
        {
            var directors = _cinemaDbContext.Set<Director>().ToList();

            if (directors.Count == 0) { return null; }

            return directors.Select(director => new Contracts.Director
            {
                DirectorUid = director.DirectorUid,
                FullName = director.FullName
            }).ToList();
        }

        public bool UpdateDirector(Guid directorUid, string fullName)
        {
            var director = _cinemaDbContext.Set<Director>().SingleOrDefault(x => x.DirectorUid == directorUid);

            if (director == null) { return false; }

            director.FullName = fullName;

            return _cinemaDbContext.SaveChanges() > 0;
        }

        public bool DeleteDirector(Guid directorUid)
        {
            var director = _cinemaDbContext.Set<Director>().SingleOrDefault(x => x.DirectorUid == directorUid);

            if (director == null) { return false; }

            _cinemaDbContext.Remove(director);

            return _cinemaDbContext.SaveChanges() > 0;
        }

        public bool IsDirectorExists(Guid directorUid)
        {
            var director = _cinemaDbContext.Set<Director>().SingleOrDefault(x => x.DirectorUid == directorUid);

            if (director == null) { return false; }

            return true;
        }

        public bool CheckRegex(string name)
        {
            var regex = new Regex(@"^[a-zA-Zа-яА-Я][a-zA-Zа-яА-Я -]{1,}$");

            if (!regex.IsMatch(name))
            {
                return false;
            }

            return true;
        }
    }
}