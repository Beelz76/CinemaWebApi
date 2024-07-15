using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Interface;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGenre(string name)
        {
            if (!_genreService.IsValidGenreName(name))
            {
                return BadRequest("Invalid genre name format");
            }

            if (await _genreService.GenreExistsAsync(name))
            {
                return Conflict("Genre already exists");
            }

            if (!await _genreService.CreateGenreAsync(name))
            {
                return BadRequest("Failed to create genre");
            }

            return Ok("Genre created");
        }

        [HttpGet]
        public async Task<IActionResult> GetGenres()
        {
            var genres = await _genreService.GetGenresAsync();

            if (genres.Count == 0)
            {
                return NotFound("No genres found");
            }

            return Ok(genres);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateGenre(Guid genreUid, string name)
        {
            if (!_genreService.IsValidGenreName(name))
            {
                return BadRequest("Invalid genre name format");
            }

            if (await _genreService.GenreExistsAsync(name))
            {
                return Conflict("Genre already exists");
            }

            if (!await _genreService.UpdateGenreAsync(genreUid, name))
            {
                return BadRequest("Failed to update genre");
            }

            return Ok("Genre updated");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteGenre(Guid genreUid)
        {
            if (!await _genreService.DeleteGenreAsync(genreUid))
            {
                return BadRequest("Failed to delete genre");
            }

            return Ok("Genre deleted");
        }
    }
}