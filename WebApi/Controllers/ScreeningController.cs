using WebApi.Contracts;
using WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ScreeningController : ControllerBase
    {
        private readonly ScreeningService _screeningService;
        private readonly MovieService _movieService;
        private readonly HallService _hallService;
        private readonly ScreeningPriceService _screeningPriceService;

        public ScreeningController(ScreeningService screeningService, MovieService movieService, HallService hallService, ScreeningPriceService screeningPriceService)
        {
            _screeningService = screeningService;
            _movieService = movieService;
            _hallService = hallService;
            _screeningPriceService = screeningPriceService;
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public ActionResult CreateScreening(ScreeningInfo screeningInfo)
        {
            if (screeningInfo.MovieTitle == null || screeningInfo.HallName == null ||
                screeningInfo.ScreeningStart == null || screeningInfo.Price <= 0)
            {
                return BadRequest();
            }

            if (!_movieService.CheckMovieTitle(screeningInfo.MovieTitle))
            {
                return NotFound("Movie not found");
            }

            if (!_hallService.CheckHallName(screeningInfo.HallName))
            {
                return NotFound("Hall not found");
            }

            if (!_screeningPriceService.CheckScreeningPrice(screeningInfo.Price))
            {
                return NotFound("Price not found");
            }

            if (!_screeningService.CheckScreeningCreating(screeningInfo.MovieTitle, screeningInfo.HallName, screeningInfo.ScreeningStart))
            {
                ModelState.AddModelError("", "Failed to create screening at this time");

                return BadRequest(ModelState);
            }

            if (!_screeningService.CreateScreening(screeningInfo))
            {
                ModelState.AddModelError("", "Failed to create screening");

                return BadRequest(ModelState);
            }

            return Ok("Screening created");
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public ActionResult<List<Screening>> GetAllScreenings()
        {
            var screenings = _screeningService.GetAllScreenings();

            if (screenings == null)
            {
                return NotFound("No screenings found");
            }

            return Ok(screenings);
        }

        [HttpGet]
        //[Authorize(Roles = "Admin, User")]
        public ActionResult<List<MovieScreening>> GetMovieScreenings(Guid movieUid)
        {
            if (!_movieService.IsMovieExists(movieUid))
            {
                return NotFound("Movie not found");
            }

            var screenings = _screeningService.GetMovieScreenings(movieUid);

            if (screenings == null)
            {
                return NotFound("No screenings found");
            }

            return Ok(screenings);
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public ActionResult<List<Screening>> GetHallScreenings(string hallName)
        {
            if (!_hallService.CheckHallName(hallName))
            {
                return NotFound("Hall not found");
            }

            var screenings = _screeningService.GetHallScreenings(hallName);

            if (screenings == null)
            {
                return NotFound("No screenings found");
            }

            return Ok(screenings);
        }

        [HttpPut]
        //[Authorize(Roles = "Admin")]
        public ActionResult UpdateScreening(Guid screeningUid, ScreeningInfo screeningInfo)
        {
            if (screeningInfo.MovieTitle == null || screeningInfo.HallName == null ||
                screeningInfo.ScreeningStart == null || screeningInfo.Price <= 0)
            {
                return BadRequest();
            }

            if (!_screeningService.IsScreeningExists(screeningUid))
            {
                return NotFound("Screening not found");
            }

            if (!_movieService.CheckMovieTitle(screeningInfo.MovieTitle))
            {
                return NotFound("Movie not found");
            }

            if (!_hallService.CheckHallName(screeningInfo.HallName))
            {
                return NotFound("Hall not found");
            }

            if (!_screeningPriceService.CheckScreeningPrice(screeningInfo.Price))
            {
                return NotFound("Price not found");
            }

            if (!_screeningService.CheckScreeningCreating(screeningInfo.MovieTitle, screeningInfo.HallName, screeningInfo.ScreeningStart))
            {
                ModelState.AddModelError("", "Failed to create screening at this time");
                return BadRequest(ModelState);
            }

            if (!_screeningService.UpdateScreening(screeningUid, screeningInfo))
            {
                ModelState.AddModelError("", "Failed to update screening");

                return BadRequest(ModelState);
            }

            return Ok("Screening updated");
        }

        [HttpDelete]
        //[Authorize(Roles = "Admin")]
        public ActionResult DeleteScreening(Guid screeningUid)
        {
            if (!_screeningService.DeleteScreening(screeningUid))
            {
                ModelState.AddModelError("", "Failed to delete screening");

                return BadRequest(ModelState);
            }

            return Ok("Screening deleted");
        }
    }
}
