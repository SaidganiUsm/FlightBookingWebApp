using MediatR;

namespace FlightBookingApp.Application.Features.Tickets.Command.Update
{
    public class UpdateTicketCommand : IRequest<UpdateTicketResponse>
    {
        public int Id { get; set; }
        public string? TicketRank { get; set; }
        public double? Price { get; set; }
    }

    public class UpdateTicketCommandHandler
        : IRequestHandler<UpdateTicketCommand, UpdateTicketResponse>
    {
        public Task<UpdateTicketResponse> Handle(
            UpdateTicketCommand request, 
            CancellationToken cancellationToken
        )
        {
            throw new NotImplementedException();
        }
    }
}
