using FlightBookingApp.Application.Common.DTOs;

namespace FlightBookingApp.Application.Features.Tickets.ConfiguredResponseModel
{
    public class FlightResponseModelForTicket
    {
        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public FlightStatusDto? FlightStatus { get; set; }

        public LocationDto? DepartureLocation { get; set; }

        public LocationDto? DestinationLocation { get; set; }
    }
}
