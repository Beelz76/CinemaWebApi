using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Interface;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class ScreeningPriceController : ControllerBase
    {
        private readonly IScreeningPriceService _screeningPriceService;

        public ScreeningPriceController(IScreeningPriceService screeningPriceService)
        {
            _screeningPriceService = screeningPriceService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateScreeningPrice(int price)
        {
            if (price <= 0)
            {
                return BadRequest("Invalid format");
            }

            if (await _screeningPriceService.ScreeningPriceExistsAsync(price))
            {
                return Conflict("Screening price already exists");
            }

            if (!await _screeningPriceService.CreateScreeningPriceAsync(price))
            {
                return BadRequest("Failed to create screening price");
            }

            return Ok("Screening price created");
        }

        [HttpGet]
        public async Task<IActionResult> GetScreeningPrices()
        {
            var screeningPrices = await _screeningPriceService.GetScreeningPricesAsync();

            if (screeningPrices.Count == 0)
            {
                return NotFound("No screening prices found");
            }

            return Ok(screeningPrices);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateScreeningPrice(Guid screeningPriceUid, int price)
        {
            if (price <= 0)
            {
                return BadRequest("Invalid format");
            }

            if (await _screeningPriceService.ScreeningPriceExistsAsync(price))
            {
                return Conflict("Screening price already exists");
            }

            if (!await _screeningPriceService.UpdateScreeningPriceAsync(screeningPriceUid, price))
            {
                return BadRequest("Failed to update screening price");
            }

            return Ok("Screening price updated");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteScreeningPrice(Guid screeningPriceUid)
        {
            if (!await _screeningPriceService.DeleteScreeningPriceAsync(screeningPriceUid))
            {
                return BadRequest("Failed to delete screening price");
            }

            return Ok("Screening price deleted");
        }
    }
}