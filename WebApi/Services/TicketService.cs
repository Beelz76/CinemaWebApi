using DatabaseAccessLayer;
using DatabaseAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using WebApi.Interface;

namespace WebApi.Services
{
    public class TicketService : ITicketService
    {
        private readonly CinemaDbContext _cinemaDbContext;

        public TicketService(CinemaDbContext cinemaDbContext)
        {
            _cinemaDbContext = cinemaDbContext;
        }

        public bool CreateTicket(Guid userUid, Guid screeningUid, Guid seatUid)
        {
            var user = _cinemaDbContext.Set<User>().SingleOrDefault(x => x.UserUid == userUid);
            var screening = _cinemaDbContext.Set<Screening>().SingleOrDefault(x => x.ScreeningUid == screeningUid);
            var seat = _cinemaDbContext.Set<Seat>().SingleOrDefault(x => x.SeatUid == seatUid);

            if (user == null || screening == null || seat == null) { return false; }

            var ticket = new Ticket
            {
                TicketUid = Guid.NewGuid(),
                User = user,
                Screening = screening,
                Seat = seat
            };

            _cinemaDbContext.Add(ticket);

            return _cinemaDbContext.SaveChanges() > 0;
        }

        public List<Contracts.Ticket>? GetAllTickets()
        {
            var tickets = _cinemaDbContext.Set<Ticket>()
                .Include(x => x.Screening.Hall)
                .Include(x => x.Screening.ScreeningPrice)
                .Include(x => x.Screening.Movie)
                .Include(x => x.Seat)
                .Include(x => x.User)
                .ToList();

            if (tickets.Count == 0) { return null; }
            
            return tickets.Select(ticket => new Contracts.Ticket
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
            }).ToList();
        }

        public List<Contracts.UserTicket>? GetUserTickets(Guid userUid)
        {
            var tickets = _cinemaDbContext.Set<Ticket>()
                .Include(x => x.Screening.Hall)
                .Include(x => x.Screening.ScreeningPrice)
                .Include(x => x.Screening.Movie)
                .Include(x => x.Seat)
                .Include(x => x.User)
                .Where(x => x.User.UserUid == userUid)
                .ToList();

            if (tickets.Count == 0) { return null; }

            return tickets.Select(ticket => new Contracts.UserTicket
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
            }).ToList();
        }

        public List<Contracts.Ticket>? GetScreeningTickets(Guid screeningUid)
        {
            var tickets = _cinemaDbContext.Set<Ticket>()
                .Include(x => x.Screening.Hall)
                .Include(x => x.Screening.ScreeningPrice)
                .Include(x => x.Screening.Movie)
                .Include(x => x.Seat)
                .Include(x => x.User)
                .Where(x => x.Screening.ScreeningUid == screeningUid)
                .ToList();

            if (tickets.Count == 0) { return null; }

            return tickets.Select(ticket => new Contracts.Ticket
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
            }).ToList();
        }

        public bool DeleteTicket(Guid ticketUid)
        {
            var ticket = _cinemaDbContext.Set<Ticket>().SingleOrDefault(x => x.TicketUid == ticketUid);

            if (ticket == null) { return false; }

            _cinemaDbContext.Remove(ticket);

            return _cinemaDbContext.SaveChanges() > 0;
        }

        public bool IsTicketExists(Guid ticketUid)
        {
            var ticket = _cinemaDbContext.Set<Ticket>().SingleOrDefault(x => x.TicketUid == ticketUid);

            if (ticket == null) { return false; }

            return true;
        }

        public bool IsSeatTaken(Guid screeningUid, Guid seatUid)
        {
            var ticket = _cinemaDbContext.Set<Ticket>().SingleOrDefault(x => x.Screening.ScreeningUid == screeningUid && x.Seat.SeatUid == seatUid);

            if (ticket == null) { return true; };

            return false;
        }

        public bool CheckScreeningSeatExists(Guid screeningUid, Guid seatUid)
        {
            var screening = _cinemaDbContext.Set<Screening>().Include(x => x.Hall.Seats).SingleOrDefault(x => x.ScreeningUid == screeningUid);
            var seat = _cinemaDbContext.Set<Seat>().SingleOrDefault(x => x.SeatUid == seatUid);

            foreach (var item in screening.Hall.Seats)
            {
                if (item == seat) 
                { 
                    return true; 
                }
            }

            return false;
        }
    }
}