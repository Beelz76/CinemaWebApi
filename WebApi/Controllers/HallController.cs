using WebApi.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Interfaces;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class HallController : ControllerBase
    {
        private readonly IHallService _hallService;

        public HallController(IHallService hallService)
        {
            _hallService = hallService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateHall(string name)
        {
            if (!_hallService.IsValidHallName(name))
            {
                return BadRequest("Invalid hall name format");
            }

            if (await _hallService.HallExistsAsync(name))
            {
                return Conflict("Hall already exists");
            }

            if (!await _hallService.CreateHallAsync(name))
            {
                return BadRequest("Failed to create hall");
            }

            return Ok("Hall created");
        }

        [HttpGet]
        public async Task<IActionResult> GetHalls()
        {
            var halls = await _hallService.GetHallsAsync();

            if (halls.Count == 0)
            {
                return NotFound("No halls found");
            }

            return Ok(halls);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateHall(Guid hallUid, string name)
        {
            if (!_hallService.IsValidHallName(name))
            {
                return BadRequest("Invalid hall name format");
            }

            if (await _hallService.HallExistsAsync(name))
            {
                return Conflict("Hall already exists");
            }

            if (!await _hallService.UpdateHallAsync(hallUid, name))
            {
                return BadRequest("Failed to update hall");
            }

            return Ok("Hall updated");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteHall(Guid hallUid)
        {
            if (!await _hallService.DeleteHallAsync(hallUid))
            {
                return BadRequest("Failed to delete hall");
            }

            return Ok("Hall deleted");
        }
    }
}