﻿using WebApi.Contracts;
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
        public ActionResult CreateScreeningPrice(int price)
        {
            if (price <= 0)
            {
                return BadRequest();
            }

            if (_screeningPriceService.CheckScreeningPrice(price))
            {
                ModelState.AddModelError("", "Screening price already exists");

                return BadRequest(ModelState);
            }

            if (!_screeningPriceService.CreateScreeningPrice(price))
            {
                ModelState.AddModelError("", "Failed to create screening price");

                return BadRequest(ModelState);
            }

            return Ok("Screening price created");
        }

        [HttpGet]
        public ActionResult<List<ScreeningPrice>> GetScreeningPrices()
        {
            var screeningPrices = _screeningPriceService.GetScreeningPrices();

            if (screeningPrices == null)
            {
                return NotFound("No screening prices found");
            }

            return Ok(screeningPrices);
        }

        [HttpPut]
        public ActionResult UpdateScreeningPrice(Guid screeningPriceUid, int price)
        {
            if (price <= 0)
            {
                return BadRequest();
            }

            if (_screeningPriceService.CheckScreeningPrice(price))
            {
                ModelState.AddModelError("", "Screening price already exists");

                return BadRequest(ModelState);
            }

            if (!_screeningPriceService.UpdateScreeningPrice(screeningPriceUid, price))
            {
                ModelState.AddModelError("", "Failed to update screening price");

                return BadRequest(ModelState);
            }

            return Ok("Screening price updated");
        }

        [HttpDelete]
        public ActionResult DeleteScreeningPrice(Guid screeningPriceUid)
        {
            if (!_screeningPriceService.DeleteScreeningPrice(screeningPriceUid))
            {
                ModelState.AddModelError("", "Failed to delete screening price");

                return BadRequest(ModelState);
            }

            return Ok("Screening price deleted");
        }
    }
}
