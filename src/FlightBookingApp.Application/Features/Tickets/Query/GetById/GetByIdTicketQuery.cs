using AutoMapper;
using FlightBookingApp.Application.Common.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FlightBookingApp.Application.Features.Tickets.Query.GetById
{
	public class GetByIdTicketQuery : IRequest<GetByIdTicketResponse>
	{
		public int Id { get; set; }
	}

	public class GetByIdTicketQueryHandler
		: IRequestHandler<GetByIdTicketQuery, GetByIdTicketResponse>
	{
		private readonly ITicketRepository _ticketRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IFlightStatusRepository _flightStatusRepository;
        private readonly IMapper _mapper;

        public GetByIdTicketQueryHandler(
            ITicketRepository ticketRepository,
            ILocationRepository locationRepository,
            IFlightStatusRepository flightStatusRepository,
            IMapper mapper
        )
        {
			_ticketRepository = ticketRepository;
            _locationRepository = locationRepository;
            _flightStatusRepository = flightStatusRepository;
            _mapper = mapper;
        }

        public async Task<GetByIdTicketResponse> Handle(
			GetByIdTicketQuery request, 
			CancellationToken cancellationToken
        )
		{
            var ticket = await _ticketRepository.GetAsync(
                predicate: x => x.Id == request.Id,
                include: x => x.Include(t => t.Rank)
                    .Include(t => t.Flight)
                    .Include(t => t.TicketStatus)!,
                cancellationToken: cancellationToken
            );

            var departure = await _locationRepository.GetAsync(
                    x => x.Id == ticket.Flight!.DepartureLocationId,
                    cancellationToken: cancellationToken
                );

            ticket.Flight!.DepartureLocation = departure;

            var destination = await _locationRepository.GetAsync(
                x => x.Id == ticket.Flight.DestinationLocationId,
                cancellationToken: cancellationToken
            );

            ticket.Flight!.DestinationLocation = destination;

            var status = await _flightStatusRepository.GetAsync(
                x => x.Id == ticket.Flight.FlightStatusId,
                cancellationToken: cancellationToken
            );

            ticket.Flight!.FlightStatus = status;

            var response = _mapper.Map<GetByIdTicketResponse>(ticket);

            return response;
        }
	}
}
