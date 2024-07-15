using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Interface;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class DirectorController : ControllerBase
    {
        private readonly IDirectorService _directorService;

        public DirectorController(IDirectorService directorService)
        {
            _directorService = directorService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDirector(string fullName)
        {
            if (!_directorService.IsValidDirectorName(fullName))
            {
                return BadRequest("Invalid director name format");
            }

            if (!await _directorService.CreateDirectorAsync(fullName))
            {
                return BadRequest("Failed to create director");
            }

            return Ok("Director created");
        }

        [HttpGet]
        public async Task<IActionResult> GetDirectors()
        {
            var directors = await _directorService.GetDirectorsAsync();

            if (directors.Count == 0)
            {
                return NotFound("No directors found");
            }

            return Ok(directors);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDirector(Guid directorUid, string fullName)
        {
            if (!_directorService.IsValidDirectorName(fullName))
            {
                return BadRequest("Invalid director name format");
            }

            if (!await _directorService.UpdateDirectorAsync(directorUid, fullName))
            {
                return BadRequest("Failed to update director");
            }

            return Ok("Director updated");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteDirector(Guid directorUid)
        {
            if (!await _directorService.DeleteDirectorAsync(directorUid))
            {
                return BadRequest("Failed to delete director");
            }

            return Ok("Director deleted");
        }
    }
}