using WebApi.Contracts;

namespace WebApi.Interfaces
{
    public interface ITicketService
    {
        Task<bool> CreateTicketAsync(Guid userUid, Guid screeningUid, Guid seatUid);
        Task<IReadOnlyList<TicketDto>> GetAllTicketsAsync();
        Task<IReadOnlyList<UserTicketDto>> GetUserTicketsAsync(Guid userUid);
        Task<IReadOnlyList<TicketDto>> GetScreeningTicketsAsync(Guid screeningUid);
        Task<bool> DeleteTicketAsync(Guid ticketUid);
        Task<bool> TicketExistsAsync(Guid ticketUid);
    }
}