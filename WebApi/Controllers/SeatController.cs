using WebApi.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Interfaces;

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
        public async Task<IActionResult> CreateSeat(string hallName, int row, int number)
        {
            if (string.IsNullOrWhiteSpace(hallName) || row <= 0 || number <= 0)
            {
                return BadRequest("Wrong data");
            }

            if (!await _hallService.HallExistsAsync(hallName))
            {
                return NotFound("Hall not found");
            }

            if (await _seatService.SeatExistsAsync(hallName, row, number))
            {
                return Conflict("Seat already exists");
            }

            if (!await _seatService.CreateSeatAsync(hallName, row, number))
            {
                return BadRequest("Failed to create seat");
            }

            return Ok("Seat created");
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllSeats()
        {
            var seats = await _seatService.GetAllSeatsAsync();

            if (seats.Count == 0)
            {
                return NotFound("No seats found");
            }

            return Ok(seats);
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetHallSeats(string hallName)
        {
            if (!await _hallService.HallExistsAsync(hallName))
            {
                return NotFound("Hall not found");
            }

            var seats = await _seatService.GetHallSeatsAsync(hallName);

            if (seats.Count == 0)
            {
                return NotFound("No seats found");
            }

            return Ok(seats);
        }

        [HttpGet]
        //[Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetScreeningSeats(Guid screeningUid)
        {
            if (!await _screeningService.ScreeningExistsAsync(screeningUid))
            {
                return NotFound("Screening not found");
            }

            var seats = await _seatService.GetScreeningSeatsAsync(screeningUid);

            if (seats.Count == 0)
            {
                return NotFound("No seats found");
            }

            return Ok(seats);
        }

        [HttpPut]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSeat(Guid seatUid, int row, int number)
        {
            if (row <= 0 || number <= 0)
            {
                return BadRequest("Wrond data");
            }

            if (!await _seatService.UpdateSeatAsync(seatUid, row, number))
            {
                return BadRequest("Failed to update seat");
            }

            return Ok("Seat updated");
        }

        [HttpDelete]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSeat(Guid seatUid)
        {
            if (!await _seatService.DeleteSeatAsync(seatUid))
            {
                return BadRequest("Failed to delete seat");
            }

            return Ok("Seat deleted");
        }
    }
}
