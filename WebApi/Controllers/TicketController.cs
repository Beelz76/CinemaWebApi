﻿using WebApi.Contracts;
using WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly TicketService _ticketService;
        private readonly ScreeningService _screeningService;
        private readonly UserService _userService;

        public TicketController(TicketService ticketService, ScreeningService screeningService, UserService userService)
        {
            _ticketService = ticketService;
            _screeningService = screeningService;
            _userService = userService;
        }

        [HttpPost]
        public ActionResult CreateTicket(Guid userUid, Guid screeningUid, Guid seatUid)
        {
            if (!_ticketService.IsSeatTaken(screeningUid, seatUid))
            {
                ModelState.AddModelError("", "Seat already taken");

                return BadRequest(ModelState);
            }

            if (!_ticketService.CreateTicket(userUid, screeningUid, seatUid))
            {
                ModelState.AddModelError("", "Failed to create ticket");

                return BadRequest(ModelState);
            }

            return Ok("Ticket created");
        }

        [HttpGet]
        public ActionResult<List<Ticket>> GetAllTickets()
        {
            var tickets = _ticketService.GetAllTickets();

            if (tickets == null)
            {
                return NotFound("No tickets found");
            }

            return Ok(tickets);
        }

        [HttpGet]
        public ActionResult<List<UserTicket>> GetUserTickets(Guid userUid)
        {
            if (!_userService.CheckUserExists(userUid))
            {
                return NotFound("User not found");
            }

            var tickets = _ticketService.GetUserTickets(userUid);

            if (tickets == null)
            {
                return NotFound("No tickets found");
            }

            return Ok(tickets);
        }

        [HttpGet]
        public ActionResult<List<Ticket>> GetScreeningTickets(Guid screeningUid)
        {
            if (!_screeningService.CheckScreeningExists(screeningUid))
            {
                return NotFound("Screning not found");
            }

            var tickets = _ticketService.GetScreeningTickets(screeningUid);

            if (tickets == null)
            {
                return NotFound("No tickets found");
            }

            return Ok(tickets);
        }

        [HttpDelete]
        public ActionResult DeleteTicket(Guid ticketUid)
        {
            if (!_ticketService.CheckTicketExists(ticketUid))
            {
                return NotFound("Ticket not found");
            }

            if (!_ticketService.DeleteTicket(ticketUid))
            {
                ModelState.AddModelError("", "Failed to delete ticket");

                return BadRequest(ModelState);
            }

            return Ok("Ticket deleted");
        }
    }
}
