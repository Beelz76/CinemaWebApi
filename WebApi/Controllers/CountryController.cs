﻿using WebApi.Contracts;
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
        public ActionResult CreateCountry(string name)
        {
            if (name == null)
            {
                return BadRequest();
            }

            if (!_countryService.CheckRegex(name))
            {
                ModelState.AddModelError("", "Invalid country name format");

                return BadRequest(ModelState);
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

            return Ok("Country created");
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

            if (!_countryService.CheckRegex(name))
            {
                ModelState.AddModelError("", "Invalid country name format");

                return BadRequest(ModelState);
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
            if (!_countryService.DeleteCountry(countryUid))
            {
                ModelState.AddModelError("", "Failed to delete country");

                return BadRequest(ModelState);
            }
             
            return Ok("Country deleted");
        }
    }
}
