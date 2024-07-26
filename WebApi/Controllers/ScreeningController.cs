using WebApi.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Interfaces;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ScreeningController : ControllerBase
    {
        private readonly IScreeningService _screeningService;
        private readonly IMovieService _movieService;
        private readonly IHallService _hallService;
        private readonly IScreeningPriceService _screeningPriceService;

        public ScreeningController(IScreeningService screeningService, IMovieService movieService, IHallService hallService, IScreeningPriceService screeningPriceService)
        {
            _screeningService = screeningService;
            _movieService = movieService;
            _hallService = hallService;
            _screeningPriceService = screeningPriceService;
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateScreening(ScreeningInfo screeningInfo)
        {
            if (string.IsNullOrWhiteSpace(screeningInfo.MovieTitle) || string.IsNullOrWhiteSpace(screeningInfo.HallName) ||
                string.IsNullOrWhiteSpace(screeningInfo.ScreeningStart) || screeningInfo.Price <= 0)
            {
                return BadRequest("Wrong data");
            }

            if (!await _movieService.MovieExistsByTitleAsync(screeningInfo.MovieTitle))
            {
                return NotFound("Movie not found");
            }

            if (!await _hallService.HallExistsAsync(screeningInfo.HallName))
            {
                return NotFound("Hall not found");
            }

            if (!await _screeningPriceService.ScreeningPriceExistsAsync(screeningInfo.Price))
            {
                return NotFound("Price not found");
            }

            if (!await _screeningService.IsValidScreeningTimeAsync(screeningInfo.MovieTitle, screeningInfo.HallName, screeningInfo.ScreeningStart))
            {
                return BadRequest("Failed to create screening at this time");
            }

            if (!await _screeningService.CreateScreeningAsync(screeningInfo))
            {
                return BadRequest("Failed to create screening");
            }

            return Ok("Screening created");
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllScreenings()
        {
            var screenings = await _screeningService.GetAllScreeningsAsync();

            if (screenings.Count == 0)
            {
                return NotFound("No screenings found");
            }

            return Ok(screenings);
        }

        [HttpGet]
        //[Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetMovieScreenings(Guid movieUid)
        {
            if (!await _movieService.MovieExistsAsync(movieUid))
            {
                return NotFound("Movie not found");
            }

            var screenings = await _screeningService.GetMovieScreeningsAsync(movieUid);

            if (screenings.Count == 0)
            {
                return NotFound("No screenings found");
            }

            return Ok(screenings);
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetHallScreenings(string hallName)
        {
            if (!await _hallService.HallExistsAsync(hallName))
            {
                return NotFound("Hall not found");
            }

            var screenings = await _screeningService.GetHallScreeningsAsync(hallName);

            if (screenings.Count == 0)
            {
                return NotFound("No screenings found");
            }

            return Ok(screenings);
        }

        [HttpPut]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateScreening(Guid screeningUid, ScreeningInfo screeningInfo)
        {
            if (string.IsNullOrWhiteSpace(screeningInfo.MovieTitle) || string.IsNullOrWhiteSpace(screeningInfo.HallName) ||
                string.IsNullOrWhiteSpace(screeningInfo.ScreeningStart) || screeningInfo.Price <= 0)
            {
                return BadRequest("Wrong data");
            }

            if (!await _movieService.MovieExistsByTitleAsync(screeningInfo.MovieTitle))
            {
                return NotFound("Movie not found");
            }

            if (!await _hallService.HallExistsAsync(screeningInfo.HallName))
            {
                return NotFound("Hall not found");
            }

            if (!await _screeningPriceService.ScreeningPriceExistsAsync(screeningInfo.Price))
            {
                return NotFound("Price not found");
            }

            if (!await _screeningService.IsValidScreeningTimeAsync(screeningInfo.MovieTitle, screeningInfo.HallName, screeningInfo.ScreeningStart))
            {
                return BadRequest("Failed to create screening at this time");
            }

            if (!await _screeningService.UpdateScreeningAsync(screeningUid, screeningInfo))
            {
                return BadRequest("Failed to update screening");
            }

            return Ok("Screening updated");
        }

        [HttpDelete]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteScreening(Guid screeningUid)
        {
            if (!await _screeningService.DeleteScreeningAsync(screeningUid))
            {
                return BadRequest("Failed to delete screening");
            }

            return Ok("Screening deleted");
        }
    }
}
