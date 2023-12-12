using WebApi.Contracts;
using WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly MovieService _movieService;

        public MovieController(MovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpPost]
        public ActionResult CreateMovie(MovieInfo movieInfo)
        {
            if (_movieService.CreateMovie(movieInfo))
            {
                return Ok("Success");
            }

            return BadRequest();
        }

        [HttpGet]
        public ActionResult<List<Movie>> GetAllMovies()
        {
            var movies = _movieService.GetAllMovies();

            if (movies == null)
            {
                return NotFound("No movies found");
            }

            return Ok(movies);
        }

        [HttpGet]
        public ActionResult<Movie> GetSingleMovie(Guid movieUid)
        {
            var movie = _movieService.GetSingleMovie(movieUid);

            if (movie == null)
            {
                return NotFound("Movie not found");
            }

            return Ok(movie);
        }

        [HttpGet]
        public ActionResult<List<MovieInfo>> GetMoviesInfo()
        {
            var movies = _movieService.GetMoviesInfo();

            if (movies == null)
            {
                return NotFound("No movies found");
            }

            return Ok(movies);
        }

        [HttpPut]
        public ActionResult UpdateMovie(Guid movieUid, MovieInfo movieInfo)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public ActionResult DeleteMovie(Guid movieUid)
        {
            if (_movieService.DeleteMovie(movieUid))
            {
                return Ok("Movie deleted");
            }

            return NotFound();
        }
    }
}
