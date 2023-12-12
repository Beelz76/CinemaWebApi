using WebApi.Contracts;
using WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ScreeningController : ControllerBase
    {
        private readonly ScreeningService _screeningService;

        public ScreeningController(ScreeningService screeningService)
        {
            _screeningService = screeningService;
        }

        [HttpPost]
        public ActionResult CreateScreening(string movieTitle, string hallName, DateTime screeningStart, int price)
        {
            if (_screeningService.CreateScreening(movieTitle, hallName, screeningStart, price))
            {
                return Ok("Success");
            }

            return BadRequest();
        }

        [HttpGet]
        public ActionResult<List<Screening>> GetScreenings()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public ActionResult<List<MovieScreening>> GetMovieScreenings(Guid movieUid)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public ActionResult UpdateScreening(Guid screeningGuid, ScreeningUpdate screeningUpdate)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public ActionResult DeleteScreening(Guid screeningUid)
        {
            if (_screeningService.DeleteScreening(screeningUid))
            {
                return Ok("Screening deleted");
            }

            return NotFound();
        }
    }
}
