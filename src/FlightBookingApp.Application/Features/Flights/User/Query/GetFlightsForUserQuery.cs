using AutoMapper;
using FlightBookingApp.Application.Common.DTOs;
using FlightBookingApp.Application.Common.Interfaces.Repositories;
using FlightBookingApp.Application.Common.Models.Requests;
using FlightBookingApp.Core.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FlightBookingApp.Application.Features.Flights.User.Query
{
    public class GetFlightsForUserQuery : IRequest<GetListResponseDto<GetFlightsForUserResponse>>
    {
        public PageRequest PageRequest { get; set; }
    }

    public class GetFlightsForUserQueryHandler
        : IRequestHandler<GetFlightsForUserQuery, GetListResponseDto<GetFlightsForUserResponse>>
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IMapper _mapper;

        public GetFlightsForUserQueryHandler(
            IFlightRepository flightRepository,
            IMapper mapper
        )
        {
            _flightRepository = flightRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponseDto<GetFlightsForUserResponse>> Handle(
            GetFlightsForUserQuery request, 
            CancellationToken cancellationToken
        )
        {
            var flights = await _flightRepository.GetListAsync(
               predicate: x => x.FlightStatus!.Name == FlightStatuses.Scheduled.ToString(),
               orderBy: x => x.OrderByDescending(x => x.StartDateTime),
               include: f => f.Include(f => f.Tickets)
               .Include(f => f.DepartureLocation)
               .Include(s => s.FlightStatus)
               .Include(f => f.DestinationLocation!),
               enableTracking: false,
               size: request.PageRequest.PageSize,
               index: request.PageRequest.PageIndex,
               cancellationToken: cancellationToken
            );

            var response = _mapper.Map<GetListResponseDto<GetFlightsForUserResponse>>(flights);

            return response;
        }
    }
}
