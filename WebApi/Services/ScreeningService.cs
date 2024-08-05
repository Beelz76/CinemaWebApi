using DatabaseAccessLayer;
using DatabaseAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using WebApi.Interfaces;

namespace WebApi.Services
{
    public class ScreeningService : IScreeningService
    {
        private readonly CinemaDbContext _cinemaDbContext;

        public ScreeningService(CinemaDbContext cinemaDbContext)
        {
            _cinemaDbContext = cinemaDbContext;
        }

        public async Task<bool> CreateScreeningAsync(Contracts.ScreeningInfo screeningInfo)
        {
            var movie = await _cinemaDbContext.Set<Movie>().FirstOrDefaultAsync(x => x.Title == screeningInfo.MovieTitle);
            var hall = await _cinemaDbContext.Set<Hall>().FirstOrDefaultAsync(x => x.Name == screeningInfo.HallName);
            var screeningPrice = await _cinemaDbContext.Set<ScreeningPrice>().FirstOrDefaultAsync(x => x.Price == screeningInfo.Price);

            if (hall == null || movie == null || screeningPrice == null) { return false; }

            if (!DateTime.TryParseExact(screeningInfo.ScreeningStart, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime screeningStartTime))
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

            await _cinemaDbContext.AddAsync(screening);
            return await _cinemaDbContext.SaveChangesAsync() > 0;
        }

        public async Task<IReadOnlyList<Contracts.Screening>> GetAllScreeningsAsync()
        {
            return await _cinemaDbContext.Set<Screening>()
                .Include(x => x.Hall)
                .Include(x => x.ScreeningPrice)
                .Include(x => x.Movie)               
                .OrderBy(x => x.ScreeningStart)
                .Select(screening => new Contracts.Screening
                {
                    ScreeningUid = screening.ScreeningUid,
                    MovieTitle = screening.Movie.Title,
                    MovieDuration = $"{screening.Movie.Duration / 60}ч {screening.Movie.Duration % 60}мин",
                    ScreeningStart = screening.ScreeningStart.ToString("dd.MM.yyyy HH:mm"),
                    ScreeningEnd = screening.ScreeningEnd.ToString("dd.MM.yyyy HH:mm"),
                    HallName = screening.Hall.Name,
                    Price = screening.ScreeningPrice.Price
                })
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Contracts.MovieScreening>> GetMovieScreeningsAsync(Guid movieUid)
        {
            return await _cinemaDbContext.Set<Screening>()
                .Include(x => x.Hall)
                .Include(x => x.ScreeningPrice)
                .Include(x => x.Movie)
                .Where(x => x.Movie.MovieUid == movieUid)
                .OrderBy(x => x.ScreeningStart)
                    .ThenBy(x => x.Hall.Name)
                .Select(screening => new Contracts.MovieScreening
                {
                    ScreeningUid = screening.ScreeningUid,
                    ScreeningStart = screening.ScreeningStart.ToString("dd.MM.yyyy HH:mm"),
                    ScreeningEnd = screening.ScreeningEnd.ToString("dd.MM.yyyy HH:mm"),
                    HallName = screening.Hall.Name,
                    Price = screening.ScreeningPrice.Price
                })
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Contracts.Screening>> GetHallScreeningsAsync(string hallName)
        {
            return await _cinemaDbContext.Set<Screening>()
                .Include(x => x.Hall)
                .Include(x => x.ScreeningPrice)
                .Include(x => x.Movie)
                .Where(x => x.Hall.Name == hallName)
                .OrderBy(x => x.ScreeningStart)
                .Select(screening => new Contracts.Screening
                {
                    ScreeningUid = screening.ScreeningUid,
                    MovieTitle = screening.Movie.Title,
                    MovieDuration = $"{screening.Movie.Duration / 60}ч {screening.Movie.Duration % 60}мин",
                    ScreeningStart = screening.ScreeningStart.ToString("dd.MM.yyyy HH:mm"),
                    ScreeningEnd = screening.ScreeningEnd.ToString("dd.MM.yyyy HH:mm"),
                    HallName = screening.Hall.Name,
                    Price = screening.ScreeningPrice.Price
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateScreeningAsync(Guid screeningUid, Contracts.ScreeningInfo screeningInfo)
        {
            var screening = await _cinemaDbContext.Set<Screening>().FirstOrDefaultAsync(x => x.ScreeningUid == screeningUid);

            if (screening == null) {  return false; }

            var movie = await _cinemaDbContext.Set<Movie>().FirstOrDefaultAsync(x => x.Title == screeningInfo.MovieTitle);
            var hall = await _cinemaDbContext.Set<Hall>().FirstOrDefaultAsync(x => x.Name == screeningInfo.HallName);
            var screeningPrice = await _cinemaDbContext.Set<ScreeningPrice>().FirstOrDefaultAsync(x => x.Price == screeningInfo.Price);

            if (movie == null || hall == null || screeningPrice == null) { return false;}

            if (!DateTime.TryParseExact(screeningInfo.ScreeningStart, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime screeningStartTime))
            {
                return false;
            }

            screening.Movie = movie;
            screening.Hall = hall;
            screening.ScreeningStart = screeningStartTime;
            screening.ScreeningEnd = screeningStartTime.AddMinutes(movie.Duration);
            screening.ScreeningPrice = screeningPrice;

            return await _cinemaDbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteScreeningAsync(Guid screeningUid)
        {
            var totalRows = await _cinemaDbContext.Set<Screening>()
                .Where(x => x.ScreeningUid == screeningUid)
                .ExecuteDeleteAsync();
            
            return totalRows > 0;
        }

        public async Task<bool> IsValidScreeningTimeAsync(string movieTitle, string hallName, string screeningStart)
        {
            var movie = await _cinemaDbContext.Set<Movie>().FirstOrDefaultAsync(x => x.Title == movieTitle);
            var hall = await _cinemaDbContext.Set<Hall>().FirstOrDefaultAsync(x => x.Name == hallName);

            if (movie == null || hall == null) { return false; }

            if (!DateTime.TryParseExact(screeningStart, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime screeningStartTime))
            {
                return false;
            }

            var screeningEndTime = screeningStartTime.AddMinutes(movie.Duration);

            var conflictingScreenings = await _cinemaDbContext.Set<Screening>()
                .Where(x => x.Hall == hall &&
                            ((x.ScreeningStart <= screeningStartTime && screeningStartTime <= x.ScreeningEnd) ||
                            (x.ScreeningStart <= screeningEndTime && screeningEndTime <= x.ScreeningEnd))).ToListAsync();

            if (conflictingScreenings.Count != 0)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> ScreeningExistsAsync(Guid screeningUid)
        {
            return await _cinemaDbContext.Set<Screening>().AnyAsync(x => x.ScreeningUid == screeningUid);
        }
    }
}