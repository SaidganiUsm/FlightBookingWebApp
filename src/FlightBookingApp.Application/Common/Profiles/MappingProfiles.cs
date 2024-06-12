using AutoMapper;
using FlightBookingApp.Application.Common.DTOs;
using FlightBookingApp.Core.Entities;

namespace FlightBookingApp.Application.Common.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<User, UserDto>();
            CreateMap<Flight, FlightDto>();
            CreateMap<Ticket, TicketDto>();
            CreateMap<Location, LocationDto>();
            CreateMap<Rank, RankDto>();
            CreateMap<FlightStatus, FlightStatusDto>();
        }
    }
}
