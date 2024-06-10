using AutoMapper;
using FlightBookingApp.Application.Common.Interfaces.Repositories;
using FlightBookingApp.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FlightBookingApp.Application.Features.Flights.Admin.Query.GetbyId
{
    public class GetFlightQuery : IRequest<GetFlightResponse>
    {
        public int Id { get; set; }
    }

    public class GetFlightQueryHandler : IRequestHandler<GetFlightQuery, GetFlightResponse>
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IMapper _mapper;

        public GetFlightQueryHandler(IFlightRepository flightRepository, IMapper mapper)
        {
            _flightRepository = flightRepository;
            _mapper = mapper;
        }

        public async Task<GetFlightResponse> Handle(
            GetFlightQuery request, 
            CancellationToken cancellationToken
        )
        {
            var flight = await _flightRepository.GetAsync(
                f => f.Id == request.Id,
                include: f => f.Include(f => f.Tickets)
                               .Include(f => f.DepartureLocation)
                               .Include(f => f.DestinationLocation!),
                cancellationToken: cancellationToken);

            if (flight == null)
                return null;

            return _mapper.Map<GetFlightResponse>(flight);
        }
    }
}
