using DatabaseAccessLayer;
using DatabaseAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using WebApi.Interfaces;

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

        public async Task<IReadOnlyList<Contracts.Seat>> GetAllSeatsAsync()
        {
            return await _cinemaDbContext.Set<Seat>()
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
        }

        public async Task<IReadOnlyList<Contracts.HallSeat>> GetHallSeatsAsync(string hallName)
        {
            return await _cinemaDbContext.Set<Seat>()
                .Where(x => x.Hall.Name == hallName)
                .OrderBy(x => x.Hall.HallUid)
                .ThenBy(x => x.Row)
                .ThenBy(x => x.Number)
                .Select(seat => new Contracts.HallSeat
                 {
                     SeatUid = seat.SeatUid,
                     HallName = seat.Hall.Name,
                     Row = seat.Row,
                     Number = seat.Number
                 })
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Contracts.ScreeningSeat>> GetScreeningSeatsAsync(Guid screeningUid)
        {
            var screening = await _cinemaDbContext.Set<Screening>()
                .Include(x => x.Hall)
                .FirstOrDefaultAsync(x => x.ScreeningUid == screeningUid);

            return await _cinemaDbContext.Set<Seat>()
                .Where(x => x.Hall.HallUid == screening.Hall.HallUid)
                .OrderBy(x => x.Row)
                .ThenBy(x => x.Number)
                .Select(x => new Contracts.ScreeningSeat
                {
                    SeatUid = x.SeatUid,
                    Row = x.Row,
                    Number = x.Number,
                    Status = x.Tickets.Any(t => t.Screening.ScreeningUid == screeningUid) ? "Занято" : "Свободно"
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateSeatAsync(Guid seatUid, int row, int number)
        {
            var totalRows = await _cinemaDbContext.Set<Seat>()
                .Where(x => x.SeatUid == seatUid)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(p => p.Row, row)
                    .SetProperty(p => p.Number, number));

            return totalRows > 0;
        }

        public async Task<bool> DeleteSeatAsync(Guid seatUid)
        {
            var totalRows = await _cinemaDbContext.Set<Seat>()
                .Where(x => x.SeatUid == seatUid)
                .ExecuteDeleteAsync();
            
            return totalRows > 0;
        }

        public async Task<bool> SeatExistsAsync(string hallName, int row, int number)
        {
            var hall = await _cinemaDbContext.Set<Hall>().FirstOrDefaultAsync(x => x.Name == hallName);
            return await _cinemaDbContext.Set<Seat>().AnyAsync(x => x.Hall == hall && x.Row == row && x.Number == number);
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
        
        public async Task<bool> IsSeatTakenAsync(Guid screeningUid, Guid seatUid)
        {
            return !await _cinemaDbContext.Set<Ticket>().AnyAsync(x => x.Screening.ScreeningUid == screeningUid && x.Seat.SeatUid == seatUid);
        }
    }
}