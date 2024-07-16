using WebApi.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Interface;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateMovie(MovieInfo movieInfo)
        {
            if (string.IsNullOrWhiteSpace(movieInfo.Title) || movieInfo.ReleaseYear <= 0 || 
                int.Parse(movieInfo.Duration) <= 0 || !_movieService.IsValidMovieTitle(movieInfo.Title))
            {
                return BadRequest("Invalid movie title format or wrong data");
            }

            if (!_movieService.IsValidNamesInList(movieInfo.Directors))
            {
                return BadRequest("Invalid director name format");
            }

            if (!_movieService.IsValidNamesInList(movieInfo.Countries))
            {
                return BadRequest("Invalid country name format");
            }

            if (!_movieService.IsValidNamesInList(movieInfo.Genres))
            {
                return BadRequest("Invalid genre name format");
            }

            if (await _movieService.MovieExistsByInfoAsync(movieInfo))
            {
                return Conflict("Movie already exists");
            }

            if (!await _movieService.CreateMovieAsync(movieInfo))
            {
                return BadRequest("Failed to create movie");
            }

            return Ok("Movie created");
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllMovies()
        {
            var movies = await _movieService.GetAllMoviesAsync();

            if (movies.Count == 0)
            {
                return NotFound("No movies found");
            }

            return Ok(movies);
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetSingleMovie(Guid movieUid)
        {
            var movie = await _movieService.GetSingleMovieAsync(movieUid);

            if (movie == null)
            {
                return NotFound("Movie not found");
            }

            return Ok(movie);
        }

        [HttpGet]
        //[Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetMoviesInfo()
        {
            var movies = await _movieService.GetMoviesInfoAsync();

            if (movies.Count == 0)
            {
                return NotFound("No movies found");
            }

            return Ok(movies);
        }

        [HttpPut]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateMovie(Guid movieUid, MovieInfo movieInfo)
        {
            if (string.IsNullOrWhiteSpace(movieInfo.Title) || movieInfo.ReleaseYear <= 0 || 
                int.Parse(movieInfo.Duration) <= 0 || !_movieService.IsValidMovieTitle(movieInfo.Title))
            {
                return BadRequest("Invalid movie title format or wrong data");
            }

            if (!_movieService.IsValidNamesInList(movieInfo.Directors))
            {
                return BadRequest("Invalid director name format");
            }

            if (!_movieService.IsValidNamesInList(movieInfo.Countries))
            {
                return BadRequest("Invalid country name format");
            }

            if (!_movieService.IsValidNamesInList(movieInfo.Genres))
            {
                return BadRequest("Invalid genre name format");
            }

            if (await _movieService.MovieExistsByInfoAsync(movieInfo, movieUid))
            {
                return BadRequest("Movie already exists");
            }

            if (!await _movieService.UpdateMovieAsync(movieUid, movieInfo))
            {
                return BadRequest("Failed to update movie");
            }

            return Ok("Movie updated");
        }

        [HttpDelete]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMovie(Guid movieUid)
        {
            if (!await _movieService.DeleteMovieAsync(movieUid))
            {
                return BadRequest("Failed to delete movie");
            }

            return Ok("Movie deleted");
        }
    }
}