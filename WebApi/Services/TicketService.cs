﻿using DatabaseAccessLayer;
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

        public async Task<List<Contracts.Ticket>> GetAllTicketsAsync()
        {
            return await _cinemaDbContext.Set<Ticket>()
                .Include(x => x.Screening.Hall)
                .Include(x => x.Screening.ScreeningPrice)
                .Include(x => x.Screening.Movie)
                .Include(x => x.Seat)
                .Include(x => x.User)
                .Select(ticket => new Contracts.Ticket
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

        public async Task<List<Contracts.UserTicket>> GetUserTicketsAsync(Guid userUid)
        {
            return await _cinemaDbContext.Set<Ticket>()
                .Include(x => x.Screening.Hall)
                .Include(x => x.Screening.ScreeningPrice)
                .Include(x => x.Screening.Movie)
                .Include(x => x.Seat)
                .Include(x => x.User)
                .Where(x => x.User.UserUid == userUid)
                .Select(ticket => new Contracts.UserTicket
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

        public async Task<List<Contracts.Ticket>> GetScreeningTicketsAsync(Guid screeningUid)
        {
            return await _cinemaDbContext.Set<Ticket>()
                .Include(x => x.Screening.Hall)
                .Include(x => x.Screening.ScreeningPrice)
                .Include(x => x.Screening.Movie)
                .Include(x => x.Seat)
                .Include(x => x.User)
                .Where(x => x.Screening.ScreeningUid == screeningUid)
                .Select(ticket => new Contracts.Ticket
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
            var ticket = await _cinemaDbContext.Set<Ticket>().FirstOrDefaultAsync(x => x.TicketUid == ticketUid);

            if (ticket == null) { return false; }

            _cinemaDbContext.Remove(ticket);
            return await _cinemaDbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> TicketExistsAsync(Guid ticketUid)
        {
            return await _cinemaDbContext.Set<Ticket>().AnyAsync(x => x.TicketUid == ticketUid);
        }

        public async Task<bool> IsSeatTakenAsync(Guid screeningUid, Guid seatUid)
        {
            return !await _cinemaDbContext.Set<Ticket>().AnyAsync(x => x.Screening.ScreeningUid == screeningUid && x.Seat.SeatUid == seatUid);
        }

        public async Task<bool> ScreeningSeatExistsAsync(Guid screeningUid, Guid seatUid)
        {
            var screening = await _cinemaDbContext.Set<Screening>()
                .Include(x => x.Hall.Seats)
                .FirstOrDefaultAsync(x => x.ScreeningUid == screeningUid);

            var seat = await _cinemaDbContext.Set<Seat>().FirstOrDefaultAsync(x => x.SeatUid == seatUid);

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