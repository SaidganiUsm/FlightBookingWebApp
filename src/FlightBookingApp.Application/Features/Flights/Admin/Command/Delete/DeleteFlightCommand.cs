using FlightBookingApp.Application.Common.Interfaces.Repositories;
using FlightBookingApp.Core.Entities;
using MediatR;

namespace FlightBookingApp.Application.Features.Flights.Admin.Command.Delete
{
    public class DeleteFlightCommand : IRequest<DeleteFlightResponse>
    {
        public int Id { get; set; }
    }

    public class DeleteFlightCommandHandler : IRequestHandler<DeleteFlightCommand, DeleteFlightResponse>
    {
        private readonly IFlightRepository _flightRepository;

        public DeleteFlightCommandHandler(IFlightRepository flightRepository)
        {
            _flightRepository = flightRepository;
        }

        public async Task<DeleteFlightResponse> Handle(DeleteFlightCommand request, CancellationToken cancellationToken)
        {
            var flight = await _flightRepository.GetAsync(
                f => f.Id == request.Id, 
                cancellationToken: cancellationToken
            );

            if (flight == null)
                return null!;

            await _flightRepository.DeleteAsync(flight);

            return new DeleteFlightResponse { Id = flight.Id};
        }
    }
}
