using AutoMapper;
using FlightBookingApp.Application.Common.DTOs;
using FlightBookingApp.Application.Common.Interfaces.Repositories;
using FlightBookingApp.Application.Common.Models.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FlightBookingApp.Application.Features.Flights.Admin.Query.GetAll
{
    public class GetAllFlightsQuery : IRequest<GetListResponseDto<GetAllFlightsResponse>>
    {
        public PageRequest PageRequest { get; set; }
    }

    public class GetAllFlightsQueryHandler : 
        IRequestHandler<GetAllFlightsQuery, GetListResponseDto<GetAllFlightsResponse>>
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IMapper _mapper;

        public GetAllFlightsQueryHandler(
            IFlightRepository flightRepository, 
            IMapper mapper
        )
        {
            _flightRepository = flightRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponseDto<GetAllFlightsResponse>> Handle(
            GetAllFlightsQuery request, 
            CancellationToken cancellationToken
        )
        {
            var flights = await _flightRepository.GetListAsync(
                include: f => f.Include(f => f.Tickets)
                .Include(f => f.DepartureLocation)
                .Include(f => f.DestinationLocation!),
                enableTracking: false,
                size: request.PageRequest.PageSize,
                index: request.PageRequest.PageIndex,
                cancellationToken: cancellationToken
                );

            var response = _mapper.Map<GetListResponseDto<GetAllFlightsResponse>>(flights);

            return response;
        }
    }
}
