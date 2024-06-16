using AutoMapper;
using FlightBookingApp.Application.Common.DTOs;
using FlightBookingApp.Application.Features.Flights.User.Query;
using FlightBookingApp.Core.Entities;
using FlightBookingApp.Core.Persistence.Paging;

namespace FlightBookingApp.Application.Features.Flights.User.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Flight, GetFlightsForUserResponse>().ReverseMap();
            CreateMap<IPaginate<Flight>, GetListResponseDto<GetFlightsForUserResponse>>().ReverseMap();
        }
    }
}
