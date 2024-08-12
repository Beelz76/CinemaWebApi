using DatabaseAccessLayer;
using DatabaseAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using WebApi.Contracts;
using WebApi.Interfaces;

namespace WebApi.Services
{
    public class TicketService : ITicketService
    {
        private readonly CinemaDbContext _cinemaDbContext;

        public TicketService(CinemaDbContext cinemaDbContext)
        {
            _cinemaDbContext = cinemaDbContext;
        }

        public async Task<bool> CreateTicketAsync(Guid userUid, Guid screeningUid, Guid seatUid)
        {
            var user = await _cinemaDbContext.Set<User>().FirstOrDefaultAsync(x => x.UserUid == userUid);
            var screening = await _cinemaDbContext.Set<Screening>().FirstOrDefaultAsync(x => x.ScreeningUid == screeningUid);
            var seat = await _cinemaDbContext.Set<Seat>().FirstOrDefaultAsync(x => x.SeatUid == seatUid);

            if (user == null || screening == null || seat == null) { return false; }

            var ticket = new Ticket
            {
                TicketUid = Guid.NewGuid(),
                User = user,
                Screening = screening,
                Seat = seat
            };

            await _cinemaDbContext.AddAsync(ticket);
            return await _cinemaDbContext.SaveChangesAsync() > 0;
        }

        public async Task<IReadOnlyList<TicketDto>> GetAllTicketsAsync()
        {
            return await _cinemaDbContext.Set<Ticket>()
                .Include(x => x.Screening.Hall)
                .Include(x => x.Screening.ScreeningPrice)
                .Include(x => x.Screening.Movie)
                .Include(x => x.Seat)
                .Include(x => x.User)
                .Select(ticket => new TicketDto
                {
                    TicketUid = ticket.TicketUid,
                    UserFullName = ticket.User.FullName,
                    MovieTitle = ticket.Screening.Movie.Title,
                    MovieDuration = ticket.Screening.Movie.Duration,
                    ScreeningStart = ticket.Screening.ScreeningStart.ToString("dd.MM.yyyy HH:mm"),
                    ScreeningEnd = ticket.Screening.ScreeningEnd.ToString("dd.MM.yyyy HH:mm"),
                    Price = ticket.Screening.ScreeningPrice.Price,
                    HallName = ticket.Screening.Hall.Name,
                    Row = ticket.Seat.Row,
                    Number = ticket.Seat.Number
                })
                .ToListAsync();
        }

        public async Task<IReadOnlyList<UserTicketDto>> GetUserTicketsAsync(Guid userUid)
        {
            return await _cinemaDbContext.Set<Ticket>()
                .Include(x => x.Screening.Hall)
                .Include(x => x.Screening.ScreeningPrice)
                .Include(x => x.Screening.Movie)
                .Include(x => x.Seat)
                .Include(x => x.User)
                .Where(x => x.User.UserUid == userUid)
                .Select(ticket => new UserTicketDto
                {
                    TicketUid = ticket.TicketUid,
                    MovieTitle = ticket.Screening.Movie.Title,
                    MovieDuration = $"{ticket.Screening.Movie.Duration / 60}ч {ticket.Screening.Movie.Duration % 60}мин",
                    ScreeningStart = ticket.Screening.ScreeningStart.ToString("dd.MM.yyyy HH:mm"),
                    ScreeningEnd = ticket.Screening.ScreeningEnd.ToString("dd.MM.yyyy HH:mm"),
                    Price = ticket.Screening.ScreeningPrice.Price,
                    HallName = ticket.Screening.Hall.Name,
                    Row = ticket.Seat.Row,
                    Number = ticket.Seat.Number
                })
                .ToListAsync();
        }

        public async Task<IReadOnlyList<TicketDto>> GetScreeningTicketsAsync(Guid screeningUid)
        {
            return await _cinemaDbContext.Set<Ticket>()
                .Include(x => x.Screening.Hall)
                .Include(x => x.Screening.ScreeningPrice)
                .Include(x => x.Screening.Movie)
                .Include(x => x.Seat)
                .Include(x => x.User)
                .Where(x => x.Screening.ScreeningUid == screeningUid)
                .Select(ticket => new TicketDto
                {
                    TicketUid = ticket.TicketUid,
                    UserFullName = ticket.User.FullName,
                    MovieTitle = ticket.Screening.Movie.Title,
                    MovieDuration = ticket.Screening.Movie.Duration,
                    ScreeningStart = ticket.Screening.ScreeningStart.ToString("dd.MM.yyyy HH:mm"),
                    ScreeningEnd = ticket.Screening.ScreeningEnd.ToString("dd.MM.yyyy HH:mm"),
                    Price = ticket.Screening.ScreeningPrice.Price,
                    HallName = ticket.Screening.Hall.Name,
                    Row = ticket.Seat.Row,
                    Number = ticket.Seat.Number
                })
                .ToListAsync();
        }

        public async Task<bool> DeleteTicketAsync(Guid ticketUid)
        {
            var totalRows = await _cinemaDbContext.Set<Ticket>()
                .Where(x => x.TicketUid == ticketUid)
                .ExecuteDeleteAsync();

            return totalRows > 0;
        }

        public async Task<bool> TicketExistsAsync(Guid ticketUid)
        {
            return await _cinemaDbContext.Set<Ticket>().AnyAsync(x => x.TicketUid == ticketUid);
        }
    }
}