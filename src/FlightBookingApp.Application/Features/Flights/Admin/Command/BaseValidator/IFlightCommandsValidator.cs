namespace FlightBookingApp.Application.Features.Flights.Admin.Command.BaseValidator
{
    public interface IFlightCommandsValidator
    {
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int DepartureLocationId { get; set; }
        public int DestinationLocationId { get; set; }
        public int TotalTickets { get; set; }
    }
}
