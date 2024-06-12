using AutoMapper;
using FlightBookingApp.Application.Common.Interfaces.Repositories;
using FlightBookingApp.Application.Features.Flights.Admin.Command.BaseValidator;
using FlightBookingApp.Core.Entities;
using FlightBookingApp.Core.Enums;
using MediatR;

namespace FlightBookingApp.Application.Features.Flights.Admin.Command.Create
{
    public class CreateFlightCommand : IRequest<CreateFlightResponse>, IFlightCommandsValidator
    {
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int DepartureLocationId { get; set; }
        public int DestinationLocationId { get; set; }
        public int TotalTickets { get; set; }
    }

    public class CreateFlightCommandHandler : IRequestHandler<CreateFlightCommand, CreateFlightResponse>
    {
        private readonly IFlightRepository _flightRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IFlightStatusRepository _flightStatusRepository;
        private readonly IMapper _mapper;

        public CreateFlightCommandHandler(
            IFlightRepository flightRepository, 
            ILocationRepository locationRepository,
            IFlightStatusRepository flightStatusRepository,
            IMapper mapper
        )
        {
            _flightRepository = flightRepository;
            _locationRepository = locationRepository;
            _flightStatusRepository = flightStatusRepository;
            _mapper = mapper;
        }

        public async Task<CreateFlightResponse> Handle(
            CreateFlightCommand request,
            CancellationToken cancellationToken
        )
        {
            var departure = await _locationRepository.GetAsync(
                l => l.Id == request.DepartureLocationId,
                cancellationToken: cancellationToken
            );

            var status = await _flightStatusRepository.GetAsync(
                s => s.Name == FlightStatuses.Scheduled.ToString(),
                cancellationToken: cancellationToken
            );

            var destination = await _locationRepository.GetAsync(
                l => l.Id == request.DestinationLocationId,
                cancellationToken: cancellationToken
            );

            var newFlight = new Flight
            {
                StartDateTime = request.StartDateTime,
                EndDateTime = request.EndDateTime,
                DepartureLocation = departure,
                DestinationLocation = destination,
                FlightStatus = status,
                TotalTickets = request.TotalTickets,
                TicketsAvailable = request.TotalTickets
            };

            var result = await _flightRepository.AddAsync(newFlight);

            return new CreateFlightResponse { Id = result.Id };
        }
    }
}
