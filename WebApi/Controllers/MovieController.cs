﻿using WebApi.Contracts;
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
        public ActionResult CreateMovie(MovieInfo movieInfo)
        {
            if (movieInfo.Title == null || movieInfo.ReleaseYear <= 0 || int.Parse(movieInfo.Duration) <= 0)
            {
                return BadRequest();
            }

            if (!_movieService.CheckRegex(movieInfo.Title))
            {
                ModelState.AddModelError("", "Invalid movie title format");

                return BadRequest(ModelState);
            }

            if (!_movieService.CheckRegexList(movieInfo.Directors))
            {
                ModelState.AddModelError("", "Invalid director name format");

                return BadRequest(ModelState);
            }

            if (!_movieService.CheckRegexList(movieInfo.Countries))
            {
                ModelState.AddModelError("", "Invalid country name format");

                return BadRequest(ModelState);
            }

            if (!_movieService.CheckRegexList(movieInfo.Genres))
            {
                ModelState.AddModelError("", "Invalid genre name format");

                return BadRequest(ModelState);
            }

            if (_movieService.CheckMovieInfo(movieInfo))
            {
                ModelState.AddModelError("", "Movie already exists");

                return BadRequest(ModelState);
            }

            if (!_movieService.CreateMovie(movieInfo))
            {
                ModelState.AddModelError("", "Failed to create movie");

                return BadRequest(ModelState);
            }

            return Ok("Movie created");
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
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
        //[Authorize(Roles = "Admin")]
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
        //[Authorize(Roles = "Admin, User")]
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
        //[Authorize(Roles = "Admin")]
        public ActionResult UpdateMovie(Guid movieUid, MovieInfo movieInfo)
        {
            if (movieInfo.Title == null || movieInfo.ReleaseYear <= 0 || int.Parse(movieInfo.Duration) <= 0)
            {
                return BadRequest();
            }

            if (!_movieService.CheckRegex(movieInfo.Title))
            {
                ModelState.AddModelError("", "Invalid movie title format");

                return BadRequest(ModelState);
            }

            if (!_movieService.CheckRegexList(movieInfo.Directors))
            {
                ModelState.AddModelError("", "Invalid director name format");

                return BadRequest(ModelState);
            }

            if (!_movieService.CheckRegexList(movieInfo.Countries))
            {
                ModelState.AddModelError("", "Invalid country name format");

                return BadRequest(ModelState);
            }

            if (!_movieService.CheckRegexList(movieInfo.Genres))
            {
                ModelState.AddModelError("", "Invalid genre name format");

                return BadRequest(ModelState);
            }

            if (_movieService.CheckMovieInfo(movieUid, movieInfo))
            {
                ModelState.AddModelError("", "Movie already exists");

                return BadRequest(ModelState);
            }

            if (!_movieService.UpdateMovie(movieUid, movieInfo))
            {
                ModelState.AddModelError("", "Failed to update movie");

                return BadRequest(ModelState);
            }

            return Ok("Movie updated");
        }

        [HttpDelete]
        //[Authorize(Roles = "Admin")]
        public ActionResult DeleteMovie(Guid movieUid)
        {
            if (!_movieService.DeleteMovie(movieUid))
            {
                ModelState.AddModelError("", "Failed to delete movie");

                return BadRequest(ModelState);
            }

            return Ok("Movie deleted");
        }
    }
}
