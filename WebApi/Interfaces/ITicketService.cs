namespace WebApi.Interface
{
    public interface ITicketService
    {
        bool CreateTicket(Guid userUid, Guid screeningUid, Guid seatUid);
        List<Contracts.Ticket>? GetAllTickets();
        List<Contracts.UserTicket>? GetUserTickets(Guid userUid);
        List<Contracts.Ticket>? GetScreeningTickets(Guid screeningUid);
        bool DeleteTicket(Guid ticketUid);
        bool IsTicketExists(Guid ticketUid);
        bool IsSeatTaken(Guid screeningUid, Guid seatUid);
        bool CheckScreeningSeatExists(Guid screeningUid, Guid seatUid);
    }
}