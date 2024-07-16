using WebApi.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Interface;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly IScreeningService _screeningService;
        private readonly IUserService _userService;

        public TicketController(ITicketService ticketService, IScreeningService screeningService, IUserService userService)
        {
            _ticketService = ticketService;
            _screeningService = screeningService;
            _userService = userService;
        }

        [HttpPost]
        //[Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> CreateTicket(Guid userUid, Guid screeningUid, Guid seatUid)
        {
            if (!await _userService.UserExistsAsync(userUid))
            {
                return NotFound("User not found");
            }

            if (!await _screeningService.ScreeningExistsAsync(screeningUid))
            {
                return NotFound("Screening not found");
            }

            if (!await _ticketService.ScreeningSeatExistsAsync(screeningUid, seatUid))
            {
                return NotFound("Seat not found");
            }

            if (!await _ticketService.IsSeatTakenAsync(screeningUid, seatUid))
            {
                return BadRequest("Seat already taken");
            }          

            if (!await _ticketService.CreateTicketAsync(userUid, screeningUid, seatUid))
            {
                return BadRequest("Failed to create ticket");
            }

            return Ok("Ticket created");
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllTickets()
        {
            var tickets = await _ticketService.GetAllTicketsAsync();

            if (tickets.Count == 0)
            {
                return NotFound("No tickets found");
            }

            return Ok(tickets);
        }

        [HttpGet]
        //[Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetUserTickets(Guid userUid)
        {
            if (!await _userService.UserExistsAsync(userUid))
            {
                return NotFound("User not found");
            }

            var tickets = await _ticketService.GetUserTicketsAsync(userUid);

            if (tickets.Count == 0)
            {
                return NotFound("No tickets found");
            }

            return Ok(tickets);
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetScreeningTickets(Guid screeningUid)
        {
            if (!await _screeningService.ScreeningExistsAsync(screeningUid))
            {
                return NotFound("Screning not found");
            }

            var tickets = await _ticketService.GetScreeningTicketsAsync(screeningUid);

            if (tickets.Count == 0)
            {
                return NotFound("No tickets found");
            }

            return Ok(tickets);
        }

        [HttpDelete]
        //[Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> DeleteTicket(Guid ticketUid)
        {
            if (!await _ticketService.DeleteTicketAsync(ticketUid))
            {
                return BadRequest("Failed to delete ticket");
            }

            return Ok("Ticket deleted");
        }
    }
}
