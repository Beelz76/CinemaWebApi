using WebApi.Contracts;
using WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly GenreService _genreService;

        public GenreController(GenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpPost]
        public ActionResult<Guid> CreateGenre(string name)
        {
            if (name == null)
            {
                return BadRequest();
            }

            if (_genreService.CheckGenreName(name))
            {
                ModelState.AddModelError("", "Genre already exists");

                return BadRequest(ModelState);
            }

            if (!_genreService.CreateGenre(name))
            {
                ModelState.AddModelError("", "Failed to create genre");

                return BadRequest(ModelState);
            }

            return Ok("Genre created");
        }

        [HttpGet]
        public ActionResult<List<Genre>> GetGenres()
        {
            var genres = _genreService.GetGenres();

            if (genres == null)
            {
                return NotFound("No genres found");
            }

            return Ok(genres);
        }

        [HttpPut]
        public ActionResult UpdateGenre(Guid genreUid, string name)
        {
            if (name == null)
            {
                return BadRequest();
            }

            if (!_genreService.CheckGenreExists(genreUid))
            {
                return NotFound("Genre not found");
            }

            if (_genreService.CheckGenreName(name))
            {
                ModelState.AddModelError("", "Genre already exists");

                return BadRequest(ModelState);
            }

            if (!_genreService.UpdateGenre(genreUid, name))
            {
                ModelState.AddModelError("", "Failed to update genre");

                return BadRequest(ModelState);
            }

            return Ok("Genre updated");
        }

        [HttpDelete]
        public ActionResult DeleteGenre(Guid genreUid)
        {
            if (!_genreService.CheckGenreExists(genreUid))
            {
                return NotFound("Genre not found");
            }

            if (!_genreService.DeleteGenre(genreUid))
            {
                ModelState.AddModelError("", "Failed to delete genre");

                return BadRequest(ModelState);
            }

            return Ok("Genre deleted");
        }
    }
}
