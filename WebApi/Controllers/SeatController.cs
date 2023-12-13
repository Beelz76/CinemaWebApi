using WebApi.Contracts;
using WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SeatController : ControllerBase
    {
        private readonly SeatService _seatService;
        private readonly HallService _hallService;
        private readonly ScreeningService _screeningService;

        public SeatController(SeatService seatService, HallService hallService, ScreeningService screeningService)
        {
            _seatService = seatService;
            _hallService = hallService;
            _screeningService = screeningService;
        }

        [HttpPost]
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

            if (!_seatService.CreateSeat(hallName, row, number))
            {
                ModelState.AddModelError("", "Failed to create seat");

                return BadRequest(ModelState);
            }

            return Ok("Seat created");
        }

        [HttpGet]
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
        public ActionResult<List<HallSeat>> GetHallSeats(Guid hallUid)
        {
            if (!_hallService.IsHallExists(hallUid))
            {
                return NotFound("Hall not found");
            }

            var seats = _seatService.GetHallSeats(hallUid);

            if (seats == null)
            {
                return NotFound("No seats found");
            }

            return Ok(seats);
        }

        [HttpGet]
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
