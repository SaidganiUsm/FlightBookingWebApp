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
        public int FirstClassTicketsAmout { get; set; }
        public int BusinessTicketsAmout { get; set; }
        public int EconomyTicketsAmout { get; set; }
        public int StandartPriceForFlight { get; set; }
    }

    public class CreateFlightCommandHandler : IRequestHandler<CreateFlightCommand, CreateFlightResponse>
    {
        private readonly IFlightRepository _flightRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IFlightStatusRepository _flightStatusRepository;

        public CreateFlightCommandHandler(
            IFlightRepository flightRepository, 
            ILocationRepository locationRepository,
            IFlightStatusRepository flightStatusRepository
        )
        {
            _flightRepository = flightRepository;
            _locationRepository = locationRepository;
            _flightStatusRepository = flightStatusRepository;
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

            var amount = request.FirstClassTicketsAmout + request.BusinessTicketsAmout +
                request.EconomyTicketsAmout;

            var newFlight = new Flight
            {
                StartDateTime = request.StartDateTime,
                EndDateTime = request.EndDateTime,
                DepartureLocation = departure,
                DestinationLocation = destination,
                FlightStatus = status,
                TotalTickets = amount,
                TicketsAvailable = amount,
                FirstClassTicketsAmount = request.FirstClassTicketsAmout,
                BusinessTicketsAmount = request.BusinessTicketsAmout,
                EconomyTicketsAmount = request.EconomyTicketsAmout
            };

            var result = await _flightRepository.AddAsync(newFlight);

            return new CreateFlightResponse { Id = result.Id };
        }
    }
}
