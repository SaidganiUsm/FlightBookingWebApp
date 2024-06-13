using FlightBookingApp.Application.Common.DTOs;

namespace FlightBookingApp.Application.Features.Tickets.Query.GetById
{
	public class GetByIdTicketResponse
	{
        public double? Price { get; set; }

        public FlightDto? Flight { get; set; }

        public RankDto? Rank { get; set; }

        public TicketStatusDto? TicketStatus { get; set; }
    }
}
