using AutoMapper;
using FlightBookingApp.Application.Common.DTOs;
using FlightBookingApp.Application.Features.Flights.Admin.Command.Create;
using FlightBookingApp.Application.Features.Flights.Admin.Query.GetAll;
using FlightBookingApp.Application.Features.Flights.Admin.Query.GetbyId;
using FlightBookingApp.Core.Entities;

namespace FlightBookingApp.Application.Features.Flights.Admin.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Flight, GetListResponseDto<GetAllFlightsRespnse>>();
            CreateMap<Flight, GetFlightResponse>();

            CreateMap<Flight, CreateFlightResponse>();
        }
    }
}
