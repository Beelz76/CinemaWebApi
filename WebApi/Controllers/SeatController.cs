﻿using WebApi.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Interface;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SeatController : ControllerBase
    {
        private readonly ISeatService _seatService;
        private readonly IHallService _hallService;
        private readonly IScreeningService _screeningService;

        public SeatController(ISeatService seatService, IHallService hallService, IScreeningService screeningService)
        {
            _seatService = seatService;
            _hallService = hallService;
            _screeningService = screeningService;
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public ActionResult CreateSeat(string hallName, int row, int number)
        {
            if (hallName == null || row <= 0 || number <= 0)
            {
                return BadRequest();
            }

            if (!_hallService.CheckHallName(hallName))
            {
                return NotFound("Hall not found");
            }

            if (_seatService.CheckSeat(hallName, row, number))
            {
                ModelState.AddModelError("", "Seat already exists");

                return BadRequest(ModelState);
            }

            if (!_seatService.CreateSeat(hallName, row, number))
            {
                ModelState.AddModelError("", "Failed to create seat");

                return BadRequest(ModelState);
            }

            return Ok("Seat created");
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public ActionResult<List<Seat>> GetAllSeats()
        {
            var seats = _seatService.GetAllSeats();

            if (seats == null)
            {
                return NotFound("No seats found");
            }

            return Ok(seats);
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public ActionResult<List<HallSeat>> GetHallSeats(string hallName)
        {
            if (!_hallService.CheckHallName(hallName))
            {
                return NotFound("Hall not found");
            }

            var seats = _seatService.GetHallSeats(hallName);

            if (seats == null)
            {
                return NotFound("No seats found");
            }

            return Ok(seats);
        }

        [HttpGet]
        //[Authorize(Roles = "Admin, User")]
        public ActionResult<List<ScreeningSeat>> GetScreeningSeats(Guid screeningUid)
        {
            if (!_screeningService.IsScreeningExists(screeningUid))
            {
                return NotFound("Screening not found");
            }

            var seats = _seatService.GetScreeningSeats(screeningUid);

            if (seats == null)
            {
                return NotFound("No seats found");
            }

            return Ok(seats);
        }

        [HttpPut]
        //[Authorize(Roles = "Admin")]
        public ActionResult UpdateSeat(Guid seatUid, int row, int number)
        {
            if (row <= 0 || number <= 0)
            {
                return BadRequest();
            }

            if (!_seatService.UpdateSeat(seatUid, row, number))
            {
                ModelState.AddModelError("", "Failed to update seat");

                return BadRequest(ModelState);
            }

            return Ok("Seat updated");
        }

        [HttpDelete]
        //[Authorize(Roles = "Admin")]
        public ActionResult DeleteSeat(Guid seatUid)
        {
            if (!_seatService.DeleteSeat(seatUid))
            {
                ModelState.AddModelError("", "Failed to delete seat");

                return BadRequest(ModelState);
            }

            return Ok("Seat deleted");
        }
    }
}
