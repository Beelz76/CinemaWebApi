using DatabaseAccessLayer;
using DatabaseAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace WebApi.Services
{
    public class ScreeningService
    {
        private readonly CinemaDbContext _cinemaDbContext;

        public ScreeningService(CinemaDbContext cinemaDbContext)
        {
            _cinemaDbContext = cinemaDbContext;
        }

        public bool CreateScreening(Contracts.ScreeningInfo screeningInfo)
        {
            var movie = _cinemaDbContext.Set<Movie>().SingleOrDefault(x => x.Title == screeningInfo.MovieTitle);
            var hall = _cinemaDbContext.Set<Hall>().SingleOrDefault(x => x.Name == screeningInfo.HallName);
            var screeningPrice = _cinemaDbContext.Set<ScreeningPrice>().SingleOrDefault(x => x.Price == screeningInfo.Price);

            if (hall == null || movie == null || screeningPrice == null) { return false; }

            DateTime screeningStartTime;

            if (!DateTime.TryParseExact(screeningInfo.ScreeningStart, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out screeningStartTime))
            {
                return false;
            }

            var screening = new Screening
            {
                ScreeningUid = Guid.NewGuid(),
                Movie = movie,
                Hall = hall,
                ScreeningStart = screeningStartTime,
                ScreeningEnd = screeningStartTime.AddMinutes(movie.Duration),
                ScreeningPrice = screeningPrice,
            };

            _cinemaDbContext.Add(screening);

            return _cinemaDbContext.SaveChanges() > 0;
        }

        public List<Contracts.Screening>? GetScreenings()
        {
            var screenings = _cinemaDbContext.Set<Screening>()
                .Include(x => x.Hall)
                .Include(x => x.ScreeningPrice)
                .Include(x => x.Movie)
                .ToList();
            
            if (screenings.Count == 0) { return null; }

            return screenings.Select(screening => new Contracts.Screening
            {
                ScreeingUid = screening.ScreeningUid,
                MovieTitle = screening.Movie.Title,
                MovieDuration = screening.Movie.Duration,
                ScreeningStart = screening.ScreeningStart.ToString("dd.MM.yyyy HH:mm"),
                ScreeningEnd = screening.ScreeningEnd.ToString("dd.MM.yyyy HH:mm"),
                HallName = screening.Hall.Name,
                Price = screening.ScreeningPrice.Price
            }).ToList();
        }

        public List<Contracts.MovieScreening>? GetMovieScreenings(Guid movieUid)
        {
            var screenings = _cinemaDbContext.Set<Screening>()
                .Include(x => x.Hall)
                .Include(x => x.ScreeningPrice)
                .Include(x => x.Movie)
                .Where(x => x.Movie.MovieUid == movieUid)
                .ToList();

            if (screenings.Count == 0) { return null; }

            return screenings.Select(screening => new Contracts.MovieScreening
            {
                //MovieTitle = screening.Movie.Title,
                //MovieDuration = screening.Movie.Duration,
                ScreeningStart = screening.ScreeningStart.ToString("dd.MM.yyyy HH:mm"),
                ScreeningEnd = screening.ScreeningEnd.ToString("dd.MM.yyyy HH:mm"),
                HallName = screening.Hall.Name,
                Price = screening.ScreeningPrice.Price
            }).ToList();
        }

        public bool UpdateScreening(Guid screeningGuid, Contracts.ScreeningInfo screeningInfo)
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

        public bool CheckScreeningCreating(string movieTitle, string hallName, string screeningStart)
        {
            var movie = _cinemaDbContext.Set<Movie>().SingleOrDefault(x => x.Title == movieTitle);
            var hall = _cinemaDbContext.Set<Hall>().SingleOrDefault(x => x.Name == hallName);

            if (movie == null || hall == null) { return false; };

            DateTime screeningStartTime;

            if (!DateTime.TryParseExact(screeningStart, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out screeningStartTime))
            {
                return false;
            }

            var screeningEndTime = screeningStartTime.AddMinutes(movie.Duration);

            var conflictingScreenings = _cinemaDbContext.Set<Screening>()
                .Where(x => x.Hall == hall && 
                            ((x.ScreeningStart <= screeningStartTime && screeningStartTime <= x.ScreeningEnd) ||
                            (x.ScreeningStart <= screeningEndTime && screeningEndTime <= x.ScreeningEnd))).ToList();

            if (conflictingScreenings.Any())
            {
                return false;
            }

            return true;
        }

        public bool CheckScreeningExists(Guid screeningUid)
        {
            var screening = _cinemaDbContext.Set<Screening>().SingleOrDefault(x => x.ScreeningUid == screeningUid);

            if (screening == null) { return false; }

            return true;
        }
    }
}
