using AutoMapper;
using FlightBookingApp.Application.Common.DTOs;
using FlightBookingApp.Application.Common.Interfaces.Repositories;
using FlightBookingApp.Application.Common.Interfaces.Services;
using FlightBookingApp.Application.Common.Models.Requests;
using FlightBookingApp.Core.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FlightBookingApp.Application.Features.Tickets.Query.GetAll
{
    public class GetAllTicketsQuery : IRequest<GetListResponseDto<GetAllTicketsResponse>>
    {
        public PageRequest PageRequest { get; set; }
    }

    public class GetAllTicketsQueryHandler
        : IRequestHandler<GetAllTicketsQuery, GetListResponseDto<GetAllTicketsResponse>>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILocationRepository _locationRepository;
        private readonly IFlightStatusRepository _flightStatusRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public GetAllTicketsQueryHandler(
            ITicketRepository ticketRepository,
            ICurrentUserService currentUserService,
            ILocationRepository locationRepository,
            IFlightStatusRepository flightStatusRepository,
            UserManager<User> userManager,
            IMapper mapper
        )
        {
            _ticketRepository = ticketRepository;
            _currentUserService = currentUserService;
            _locationRepository = locationRepository;
            _flightStatusRepository = flightStatusRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<GetListResponseDto<GetAllTicketsResponse>> Handle(
            GetAllTicketsQuery request, 
            CancellationToken cancellationToken
        )
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(
                u => u.Email == _currentUserService.UserEmail! && !u.IsDeleted,
                cancellationToken: cancellationToken
            );

            if (user == null)
                throw new ValidationException("User not found");

            var tickets = await _ticketRepository.GetListAsync(
                predicate: x => x.UserId == user.Id,
                include: x => x.Include(t => t.Rank)
                    .Include(t => t.Flight)
                    .Include(t => t.TicketStatus)!,
                enableTracking: false,
                size: request.PageRequest.PageSize,
                index: request.PageRequest.PageIndex,
                cancellationToken: cancellationToken
            );

            foreach ( var ticket in tickets.Items)
            {
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
            }

            var response = _mapper.Map<GetListResponseDto<GetAllTicketsResponse>>(tickets);

            return response;
        }
    }
}
