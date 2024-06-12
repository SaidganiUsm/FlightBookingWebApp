using AutoMapper;
using FlightBookingApp.Application.Common.Interfaces.Repositories;
using FlightBookingApp.Core.Entities;
using FlightBookingApp.Core.Enums;
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
        private readonly IFlightStatusRepository _flightStatusRepository;
        private readonly IMapper _mapper;

        public DeleteFlightCommandHandler(
            IFlightRepository flightRepository, 
            IFlightStatusRepository flightStatusRepository,
            IMapper mapper
        )
        {
            _flightRepository = flightRepository;
            _flightStatusRepository = flightStatusRepository;
            _mapper = mapper;
        }

        public async Task<DeleteFlightResponse> Handle(DeleteFlightCommand request, CancellationToken cancellationToken)
        {
            var status = await _flightStatusRepository.GetAsync(
                s => s.Name == FlightStatuses.Cancelled.ToString(),
                cancellationToken: cancellationToken
            ); 

            var flight = await _flightRepository.GetAsync(
                f => f.Id == request.Id, 
                cancellationToken: cancellationToken
            );

            if (flight == null)
                return null!;

            flight.FlightStatus = status;

            await _flightRepository.UpdateAsync(flight);

            return new DeleteFlightResponse { Id = flight.Id};
        }
    }
}
