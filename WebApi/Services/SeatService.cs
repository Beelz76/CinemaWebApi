using DatabaseAccessLayer;
using DatabaseAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using WebApi.Interface;

namespace WebApi.Services
{
    public class SeatService : ISeatService
    {
        private readonly CinemaDbContext _cinemaDbContext;

        public SeatService(CinemaDbContext cinemaDbContext)
        {
            _cinemaDbContext = cinemaDbContext;
        }

        public async Task<bool> CreateSeatAsync(string hallName, int row, int number)
        {
            var hall = await _cinemaDbContext.Set<Hall>().FirstOrDefaultAsync(x => x.Name == hallName);

            if (hall == null) { return false; }

            var seat = new Seat
            {
                SeatUid = Guid.NewGuid(),
                Hall = hall,
                Row = row,
                Number = number
            };

            hall.Capacity += 1;

            await _cinemaDbContext.AddAsync(seat);
            return await _cinemaDbContext.SaveChangesAsync() > 0;
        }

        public async Task<List<Contracts.Seat>> GetAllSeatsAsync()
        {
            var seats = await _cinemaDbContext.Set<Seat>()
                .Include(x => x.Hall)
                .OrderBy(x => x.Hall.Name)
                .ThenBy(x => x.Row)
                .ThenBy(x => x.Number)
                .Select(seat => new Contracts.Seat
                {
                    SeatUid = seat.SeatUid,
                    HallName = seat.Hall.Name,
                    Row = seat.Row,
                    Number = seat.Number
                })
                .ToListAsync();

            if (seats.Count == 0) { return new List<Contracts.Seat>(); }

            return seats;
        }

        public async Task<List<Contracts.HallSeat>> GetHallSeatsAsync(string hallName)
        {
            var seats = await _cinemaDbContext.Set<Seat>()
                .Where(x => x.Hall.Name.ToLower() == hallName.ToLower())
                .OrderBy(x => x.Hall.HallUid)
                .ThenBy(x => x.Row)
                .ThenBy(x => x.Number)
                .Select(seat => new Contracts.HallSeat
                 {
                     SeatUid = seat.SeatUid,
                     Row = seat.Row,
                     Number = seat.Number
                 })
                .ToListAsync();

            if (seats.Count == 0) { return new List<Contracts.HallSeat>(); }

            return seats;
        }

        public async Task<List<Contracts.ScreeningSeat>> GetScreeningSeatsAsync(Guid screeningUid)
        {
            var screening = await _cinemaDbContext.Set<Screening>()
                .Include(x => x.Hall)
                .FirstOrDefaultAsync(x => x.ScreeningUid == screeningUid);

            var seats = await _cinemaDbContext.Set<Seat>()
                .Where(x => x.Hall.HallUid == screening.Hall.HallUid)
                .OrderBy(x => x.Row)
                .ThenBy(x => x.Number)
                .Select(x => new Contracts.ScreeningSeat
                {
                    SeatUid = x.SeatUid,
                    Row = x.Row,
                    Number = x.Number,
                    Status = x.Tickets.Any(x => x.Screening.ScreeningUid == screeningUid) ? "Занято" : "Свободно"
                })
                .ToListAsync();

            if (seats.Count == 0) { return new List<Contracts.ScreeningSeat>(); }

            return seats;
        }

        public async Task<bool> UpdateSeatAsync(Guid seatUid, int row, int number)
        {
            var seat = await _cinemaDbContext.Set<Seat>().FirstOrDefaultAsync(x => x.SeatUid == seatUid);

            if (seat == null) { return false; }

            seat.Row = row;
            seat.Number = number;

            return await _cinemaDbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteSeatAsync(Guid seatUid)
        {
            var seat = await _cinemaDbContext.Set<Seat>().Include(x => x.Hall).FirstOrDefaultAsync(x => x.SeatUid == seatUid);

            if (seat == null) { return false; }

            seat.Hall.Capacity -= 1;

            _cinemaDbContext.Remove(seat);
            return await _cinemaDbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> SeatExistsAsync(string hallName, int row, int number)
        {
            var hall = await _cinemaDbContext.Set<Hall>().FirstOrDefaultAsync(x => x.Name == hallName);
            var seat = await _cinemaDbContext.Set<Seat>().FirstOrDefaultAsync(x => x.Hall == hall & x.Row == row & x.Number == number);

            if (seat == null) { return false; }

            return true;
        }
    }
}