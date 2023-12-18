using DatabaseAccessLayer;
using DatabaseAccessLayer.Entities;
using System.Text.RegularExpressions;
using WebApi.Interface;

namespace WebApi.Services
{
    public class HallService : IHallService
    {
        private readonly CinemaDbContext _cinemaDbContext;

        public HallService(CinemaDbContext cinemaDbContext)
        {
            _cinemaDbContext = cinemaDbContext;
        }

        public bool CreateHall(string name)
        {
            var hall = new Hall
            {
                HallUid = Guid.NewGuid(),
                Name = name,
                Capacity = 0,
            };

            _cinemaDbContext.Add(hall);

            return _cinemaDbContext.SaveChanges() > 0;
        }

        public List<Contracts.Hall>? GetHalls()
        {
            var halls = _cinemaDbContext.Set<Hall>().ToList();

            if (halls.Count == 0) { return null; }

            return halls.Select(hall => new Contracts.Hall
            {
                HallUid = hall.HallUid,
                Name = hall.Name,
                Capacity = hall.Capacity
            }).ToList();
        }

        public bool UpdateHall(Guid hallUid, string name)
        {
            var hall = _cinemaDbContext.Set<Hall>().SingleOrDefault(x => x.HallUid == hallUid);

            if (hall == null) { return false; }

            hall.Name = name;

            return _cinemaDbContext.SaveChanges() > 0;
        }

        public bool DeleteHall(Guid hallUid)
        {
            var hall = _cinemaDbContext.Set<Hall>().SingleOrDefault(x => x.HallUid == hallUid);

            if (hall == null) { return false; }

            _cinemaDbContext.Remove(hall);

            return _cinemaDbContext.SaveChanges() > 0;
        }

        public bool CheckHallName(string name)
        {
            var hall = _cinemaDbContext.Set<Hall>().SingleOrDefault(x => x.Name == name);

            if (hall == null) { return false; }

            return true;
        }

        public bool IsHallExists(Guid hallUid)
        {
            var hall = _cinemaDbContext.Set<Hall>().SingleOrDefault(x => x.HallUid == hallUid);

            if (hall == null) { return false; }

            return true;
        }

        public bool CheckRegex(string name)
        {
            var regex = new Regex(@"^[a-zA-Zа-яА-Я0-9][a-zA-Zа-яА-Я -]{1,}$");

            if (!regex.IsMatch(name))
            {
                return false;
            }

            return true;
        }
    }
}
