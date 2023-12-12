using DatabaseAccessLayer;
using DatabaseAccessLayer.Entities;

namespace WebApi.Services
{
    public class ScreeningService
    {
        private readonly CinemaDbContext _cinemaDbContext;

        public ScreeningService(CinemaDbContext cinemaDbContext)
        {
            _cinemaDbContext = cinemaDbContext;
        }

        public bool CreateScreening(string movieTitle, string hallName, DateTime screeningStart, int price)
        {
            var movie = _cinemaDbContext.Set<Movie>().SingleOrDefault(x => x.Title == movieTitle);
            var hall = _cinemaDbContext.Set<Hall>().SingleOrDefault(x => x.Name == hallName);
            var screeningPrice = _cinemaDbContext.Set<ScreeningPrice>().SingleOrDefault(x => x.Price == price);

            if (hall == null || movie == null || screeningPrice == null) { return false; }

            var screening = new Screening
            {
                ScreeningUid = Guid.NewGuid(),
                Movie = movie,
                Hall = hall,
                ScreeningStart = screeningStart,
                ScreeningEnd = screeningStart,
                ScreeningPrice = screeningPrice,

            };

            _cinemaDbContext.Add(screening);

            return _cinemaDbContext.SaveChanges() > 0;
        }

        public List<Contracts.Screening> GetScreenings()
        {
            throw new NotImplementedException();
        }

        public List<Contracts.MovieScreening> GetMovieScreenings(Guid movieUid)
        {
            throw new NotImplementedException();
        }

        public bool UpdateScreening(Guid screeningGuid, Contracts.ScreeningUpdate screeningUpdate)
        {
            throw new NotImplementedException();
        }

        public bool DeleteScreening(Guid screeningUid)
        {
            var screening = _cinemaDbContext.Set<Screening>().SingleOrDefault(x => x.ScreeningUid == screeningUid);

            if (screening == null) { return false; }

            _cinemaDbContext.Remove(screening);

            return _cinemaDbContext.SaveChanges() > 0;
        }
    }
}
