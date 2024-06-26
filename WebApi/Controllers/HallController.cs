﻿using WebApi.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Interface;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class HallController : ControllerBase
    {
        private readonly IHallService _hallService;

        public HallController(IHallService hallService)
        {
            _hallService = hallService;
        }

        [HttpPost]
        public ActionResult CreateHall(string name)
        {
            if (name == null)
            {
                return BadRequest();
            }

            if (!_hallService.CheckRegex(name))
            {
                ModelState.AddModelError("", "Invalid hall name format");

                return BadRequest(ModelState);
            }

            if (_hallService.CheckHallName(name))
            {
                ModelState.AddModelError("", "Hall already exists");

                return BadRequest(ModelState);
            }

            if (!_hallService.CreateHall(name))
            {
                ModelState.AddModelError("", "Failed to create hall");

                return BadRequest(ModelState);
            }

            return Ok("Hall created");
        }

        [HttpGet]
        public ActionResult<List<Hall>> GetHalls()
        {
            var halls = _hallService.GetHalls();

            if (halls == null)
            {
                return NotFound("No halls found");
            }

            return Ok(halls);
        }

        [HttpPut]
        public ActionResult UpdateHall(Guid hallUid, string name)
        {
            if (name == null)
            {
                return BadRequest();
            }

            if (!_hallService.CheckRegex(name))
            {
                ModelState.AddModelError("", "Invalid hall name format");

                return BadRequest(ModelState);
            }

            if (_hallService.CheckHallName(name))
            {
                ModelState.AddModelError("", "Hall already exists");

                return BadRequest(ModelState);
            }

            if (!_hallService.UpdateHall(hallUid, name))
            {
                ModelState.AddModelError("", "Failed to update hall");

                return BadRequest(ModelState);
            }

            return Ok("Hall updated");
        }

        [HttpDelete]
        public ActionResult DeleteHall(Guid hallUid)
        {
            if (!_hallService.DeleteHall(hallUid))
            {
                ModelState.AddModelError("", "Failed to delete hall");

                return BadRequest(ModelState);
            }

            return Ok("Hall deleted");
        }
    }
}
