using FlightBookingApp.Application.Common.DTOs;
using FlightBookingApp.Application.Features.Tickets.ConfiguredResponseModel;

namespace FlightBookingApp.Application.Features.Tickets.Query.GetAll
{
    public class GetAllTicketsResponse
    {
        public int Id { get; set; }

        public double? Price { get; set; }

        public FlightResponseModelForTicket? Flight { get; set; }

        public RankDto? Rank { get; set; }

        public TicketStatusDto? TicketStatus { get; set; }
    }
}
