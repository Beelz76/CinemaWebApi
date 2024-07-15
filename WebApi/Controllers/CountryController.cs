using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Interface;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }
    
        [HttpPost]
        public async Task<IActionResult> CreateCountry(string name)
        {
            if (!_countryService.IsValidCountryName(name))
            {
                return BadRequest("Invalid country name format");
            }

            if (await _countryService.CountryExistsAsync(name))
            {
                return Conflict("Country already exists");
            }

            if (!await _countryService.CreateCountryAsync(name))
            {
                return BadRequest("Failed to create country");
            }

            return Ok("Country created");
        }

        [HttpGet]
        public async Task<IActionResult> GetCountries()
        {
            var countries = await _countryService.GetCountriesAsync();

            if (countries.Count == 0)
            {
                return NotFound("No countries found");
            }

            return Ok(countries);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCountry(Guid countryUid, string name)
        {
            if (!_countryService.IsValidCountryName(name))
            {
                return BadRequest("Invalid country name format");
            }

            if (await _countryService.CountryExistsAsync(name))
            {
                return Conflict("Country already exists");
            }

            if (!await _countryService.UpdateCountryAsync(countryUid, name))
            {
                return BadRequest("Failed to update country");
            }

            return Ok("Country updated");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCountry(Guid countryUid)
        {
            if (!await _countryService.DeleteCountryAsync(countryUid))
            {
                return BadRequest("Failed to delete country");
            }
             
            return Ok("Country deleted");
        }
    }
}