using AutoMapper;
using FlightBookingApp.Application.Common.Interfaces.Repositories;
using FlightBookingApp.Core.Entities;
using MediatR;

namespace FlightBookingApp.Application.Features.Flights.Admin.Command.Create
{
    public class CreateFlightCommand : IRequest<CreateFlightResponse>
    {
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int DepartureLocationId { get; set; }
        public int DestinationLocationId { get; set; }
        public int TotalTickets { get; set; }
        public int TicketsAvailable { get; set; }
    }

    public class CreateFlightCommandHandler : IRequestHandler<CreateFlightCommand, CreateFlightResponse>
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IMapper _mapper;

        public CreateFlightCommandHandler(IFlightRepository flightRepository, IMapper mapper)
        {
            _flightRepository = flightRepository;
            _mapper = mapper;
        }

        public async Task<CreateFlightResponse> Handle(
            CreateFlightCommand request,
            CancellationToken cancellationToken
        )
        {
            var newFlight = _mapper.Map<Flight>( request );

            var result = await _flightRepository.AddAsync(newFlight);

            return new CreateFlightResponse { Id = result.Id};
        }
    }
}
