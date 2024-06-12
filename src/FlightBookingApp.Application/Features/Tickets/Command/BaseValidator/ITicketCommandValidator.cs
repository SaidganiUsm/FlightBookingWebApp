namespace FlightBookingApp.Application.Features.Tickets.Command.BaseValidator
{
    public interface ITicketCommandValidator
    {
        public int FlightId { get; set; }
        public string? TicketRank { get; set; }
        public double? Price { get; set; }
    }
}
