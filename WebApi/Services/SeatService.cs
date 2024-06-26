﻿using DatabaseAccessLayer;
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

        public bool CreateSeat(string hallName, int row, int number)
        {
            var hall = _cinemaDbContext.Set<Hall>().SingleOrDefault(x => x.Name == hallName);

            if (hall == null) { return false; }

            var seat = new Seat
            {
                SeatUid = Guid.NewGuid(),
                Hall = hall,
                Row = row,
                Number = number
            };

            hall.Capacity += 1;

            _cinemaDbContext.Add(seat);

            return _cinemaDbContext.SaveChanges() > 0;
        }

        public List<Contracts.Seat>? GetAllSeats()
        {
            var seats = _cinemaDbContext.Set<Seat>()
                .Include(x => x.Hall)
                .OrderBy(x => x.Hall.Name)
                .ThenBy(x => x.Row)
                .ThenBy(x => x.Number)
                .ToList();

            if (seats.Count == 0) { return null; }

            return seats.Select(seat => new Contracts.Seat
            {
                SeatUid = seat.SeatUid,
                HallName = seat.Hall.Name,
                Row = seat.Row,
                Number = seat.Number
            }).ToList();
        }

        public List<Contracts.HallSeat>? GetHallSeats(string hallName)
        {
            var seats = _cinemaDbContext.Set<Seat>()
                .Where(x => x.Hall.Name == hallName)
                .OrderBy(x => x.Hall.HallUid)
                .ThenBy(x => x.Row)
                .ThenBy(x => x.Number)
                .ToList();

            if (seats.Count == 0) { return null; }

            return seats.Select(seat => new Contracts.HallSeat
            {
                SeatUid = seat.SeatUid,
                Row = seat.Row,
                Number = seat.Number
            }).ToList();
        }

        public List<Contracts.ScreeningSeat>? GetScreeningSeats(Guid screeningUid)
        {
            var screening = _cinemaDbContext.Set<Screening>().Include(x => x.Hall).SingleOrDefault(x => x.ScreeningUid == screeningUid);

            if (screening == null) { return null; }

            var seats = _cinemaDbContext.Set<Seat>()
                .Where(x => x.Hall.HallUid == screening.Hall.HallUid)
                .Select(x => new Contracts.ScreeningSeat
                {
                    SeatUid = x.SeatUid,
                    Row = x.Row,
                    Number = x.Number,
                    Status = x.Tickets.Any(x => x.Screening.ScreeningUid == screeningUid) ? "Занято" : "Свободно"
                })
                .OrderBy(x => x.Row)
                .ThenBy(x => x.Number)
                .ToList();

            if (seats.Count == 0) { return null; }

            return seats.Select(seat => new Contracts.ScreeningSeat
            {
                SeatUid = seat.SeatUid,
                Row = seat.Row,
                Number = seat.Number,
                Status = seat.Status,
            }).ToList();
        }

        public bool UpdateSeat(Guid seatUid, int row, int number)
        {
            var seat = _cinemaDbContext.Set<Seat>().SingleOrDefault(x => x.SeatUid == seatUid);

            if (seat == null) { return false; }

            seat.Row = row;
            seat.Number = number;

            return _cinemaDbContext.SaveChanges() > 0;
        }

        public bool DeleteSeat(Guid seatUid)
        {
            var seat = _cinemaDbContext.Set<Seat>().Include(x => x.Hall).SingleOrDefault(x => x.SeatUid == seatUid);

            if (seat == null) { return false; }

            seat.Hall.Capacity -= 1;

            _cinemaDbContext.Remove(seat);

            return _cinemaDbContext.SaveChanges() > 0;
        }

        public bool IsSeatExists(Guid seatUid)
        {
            var seat = _cinemaDbContext.Set<Seat>().SingleOrDefault(x => x.SeatUid == seatUid);

            if (seat == null) { return false; }

            return true;
        }

        public bool CheckSeat(string hallName, int row, int number)
        {
            var hall = _cinemaDbContext.Set<Hall>().SingleOrDefault(x => x.Name == hallName);

            var seat = _cinemaDbContext.Set<Seat>().SingleOrDefault(x => x.Hall == hall & x.Row == row & x.Number == number);

            if (seat == null) { return false; }

            return true;
        }
    }
}