﻿namespace WebApi.Interfaces
{
    public interface ITicketService
    {
        Task<bool> CreateTicketAsync(Guid userUid, Guid screeningUid, Guid seatUid);
        Task<IReadOnlyList<Contracts.Ticket>> GetAllTicketsAsync();
        Task<IReadOnlyList<Contracts.UserTicket>> GetUserTicketsAsync(Guid userUid);
        Task<IReadOnlyList<Contracts.Ticket>> GetScreeningTicketsAsync(Guid screeningUid);
        Task<bool> DeleteTicketAsync(Guid ticketUid);
        Task<bool> TicketExistsAsync(Guid ticketUid);
    }
}