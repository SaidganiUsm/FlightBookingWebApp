using AutoMapper;
using FlightBookingApp.Application.Common.Interfaces.Repositories;
using FlightBookingApp.Application.Features.Flights.Admin.Command.BaseValidator;
using FlightBookingApp.Core.Entities;
using MediatR;

namespace FlightBookingApp.Application.Features.Flights.Admin.Command.Update
{
    public class UpdateFlightCommand : IRequest<UpdateFlightResponse>, IFlightCommandsValidator
    {
        public int Id { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int DepartureLocationId { get; set; }
        public int DestinationLocationId { get; set; }
        public int TotalTickets { get; set; }
    }

    public class UpdateFlightCommandHandler : IRequestHandler<UpdateFlightCommand, UpdateFlightResponse>
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IMapper _mapper;

        public UpdateFlightCommandHandler(IFlightRepository flightRepository, IMapper mapper)
        {
            _flightRepository = flightRepository;
            _mapper = mapper;
        }

        public async Task<UpdateFlightResponse> Handle(
            UpdateFlightCommand request, 
            CancellationToken cancellationToken)
        {
            var flight = await _flightRepository.GetAsync(
                f => f.Id == request.Id,
                cancellationToken: cancellationToken
            );

            if (flight == null)
                return null!;

            _mapper.Map(request, flight);

            await _flightRepository.UpdateAsync(flight);

            return new UpdateFlightResponse { Id = flight.Id };
        }
    }
}
