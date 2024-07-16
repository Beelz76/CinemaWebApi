namespace WebApi.Interface
{
    public interface ITicketService
    {
        Task<bool> CreateTicketAsync(Guid userUid, Guid screeningUid, Guid seatUid);
        Task<List<Contracts.Ticket>> GetAllTicketsAsync();
        Task<List<Contracts.UserTicket>> GetUserTicketsAsync(Guid userUid);
        Task<List<Contracts.Ticket>> GetScreeningTicketsAsync(Guid screeningUid);
        Task<bool> DeleteTicketAsync(Guid ticketUid);
        Task<bool> TicketExistsAsync(Guid ticketUid);
        Task<bool> IsSeatTakenAsync(Guid screeningUid, Guid seatUid);
        Task<bool> ScreeningSeatExistsAsync(Guid screeningUid, Guid seatUid);
    }
}