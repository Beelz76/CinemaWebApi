using WebApi.Contracts;
using WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly CountryService _countryService;

        public CountryController(CountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpPost]
        public ActionResult CreateCountry(string name)
        {
            if (name == null)
            {
                return BadRequest();
            }

            if (_countryService.CheckCountryName(name))
            {
                ModelState.AddModelError("", "Country already exists");

                return BadRequest(ModelState);
            }

            if (!_countryService.CreateCountry(name))
            {
                ModelState.AddModelError("", "Failed to create country");

                return BadRequest(ModelState);
            }

            return Ok("Genre created");
        }

        [HttpGet]
        public ActionResult<List<Country>> GetCountries()
        {
            var countries = _countryService.GetCountries();

            if (countries == null)
            {
                return NotFound("No countries found");
            }

            return Ok(countries);
        }

        [HttpPut]
        public ActionResult UpdateCountry(Guid countryUid, string name)
        {
            if (name == null)
            {
                return BadRequest();
            }

            if (!_countryService.CheckCountryExists(countryUid))
            {
                return NotFound("Country not found");
            }

            if (_countryService.CheckCountryName(name))
            {
                ModelState.AddModelError("", "Country already exists");

                return BadRequest(ModelState);
            }

            if (!_countryService.UpdateCountry(countryUid, name))
            {
                ModelState.AddModelError("", "Failed to update country");

                return BadRequest(ModelState);
            }

            return Ok("Country updated");
        }

        [HttpDelete]
        public ActionResult DeleteCountry(Guid countryUid)
        {
            if (!_countryService.CheckCountryExists(countryUid))
            {
                return NotFound("Country not found");
            }

            if (!_countryService.DeleteCountry(countryUid))
            {
                ModelState.AddModelError("", "Failed to delete country");

                return BadRequest(ModelState);
            }

            return Ok("Country deleted");
        }
    }
}
