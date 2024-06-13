using FlightBookingApp.Application.Common.Interfaces.Repositories;
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
        private readonly ITicketRepository _ticketRepository;
        private readonly IRankRepository _rankRepository;

        public UpdateTicketCommandHandler(
            ITicketRepository ticketRepository,
            IRankRepository rankRepository
        )
        {
            _ticketRepository = ticketRepository;
            _rankRepository = rankRepository;
        }

        public async Task<UpdateTicketResponse> Handle(
            UpdateTicketCommand request, 
            CancellationToken cancellationToken
        )
        {
            var ticket = await _ticketRepository.GetAsync(
                x => x.Id == request.Id,
                cancellationToken: cancellationToken
            );

            var newRank = await _rankRepository.GetAsync(
                x => x.RankName == request.TicketRank,
                cancellationToken: cancellationToken
            );

            ticket!.Rank = newRank;

            return new UpdateTicketResponse { Id = request.Id };
        }
    }
}
