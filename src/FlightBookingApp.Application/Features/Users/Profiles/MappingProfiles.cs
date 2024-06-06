using AutoMapper;
using FlightBookingApp.Application.Features.Users.Command.Update;
using FlightBookingApp.Core.Entities;
using FlightBookingApp.Core.Persistence.Paging;

namespace FlightBookingApp.Application.Features.Users.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, UpdatedUserResponse>().ReverseMap();
        }
    }
}
