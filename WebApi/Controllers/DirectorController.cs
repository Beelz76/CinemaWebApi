using WebApi.Contracts;
using WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DirectorController : ControllerBase
    {
        private readonly DirectorService _directorService;

        public DirectorController(DirectorService directorService)
        {
            _directorService = directorService;
        }

        [HttpPost]
        public ActionResult CreateDirector(string fullName)
        {
            if (fullName == null)
            {
                return BadRequest();
            }

            if (!_directorService.CreateDirector(fullName))
            {
                ModelState.AddModelError("", "Failed to create director");

                return BadRequest(ModelState);
            }

            return Ok("Director created");
        }

        [HttpGet]
        public ActionResult<List<Director>> GetDirectors()
        {
            var directors = _directorService.GetDirectors();

            if (directors == null)
            {
                return NotFound("No directors found");
            }

            return Ok(directors);
        }

        [HttpPut]
        public ActionResult UpdateDirector(Guid directorUid, string fullName)
        {
            if (fullName == null)
            {
                return BadRequest();
            }

            if (!_directorService.CheckDirectorExists(directorUid))
            {
                return NotFound("Director not found");
            }

            if (!_directorService.UpdateDirector(directorUid, fullName))
            {
                ModelState.AddModelError("", "Failed to update director");

                return BadRequest(ModelState);
            }

            return Ok("Director updated");
        }

        [HttpDelete]
        public ActionResult DeleteDirector(Guid directorUid)
        {
            if (!_directorService.CheckDirectorExists(directorUid))
            {
                return NotFound("Director not found");
            }

            if (!_directorService.DeleteDirector(directorUid))
            {
                ModelState.AddModelError("", "Failed to delete director");

                return BadRequest(ModelState);
            }

            return Ok("Director deleted");
        }
    }
}
