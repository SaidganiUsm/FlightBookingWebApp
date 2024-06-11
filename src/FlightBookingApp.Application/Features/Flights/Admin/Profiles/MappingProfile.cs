using AutoMapper;
using FlightBookingApp.Application.Common.DTOs;
using FlightBookingApp.Application.Features.Flights.Admin.Command.Create;
using FlightBookingApp.Application.Features.Flights.Admin.Command.Delete;
using FlightBookingApp.Application.Features.Flights.Admin.Command.Update;
using FlightBookingApp.Application.Features.Flights.Admin.Query.GetAll;
using FlightBookingApp.Application.Features.Flights.Admin.Query.GetbyId;
using FlightBookingApp.Core.Entities;
using FlightBookingApp.Core.Persistence.Paging;

namespace FlightBookingApp.Application.Features.Flights.Admin.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Flight, GetAllFlightsResponse>().ReverseMap();
            CreateMap<IPaginate<Flight>, GetListResponseDto<GetAllFlightsResponse>>().ReverseMap();
            CreateMap<Flight, GetFlightResponse>().ReverseMap();

            CreateMap<Flight, CreateFlightResponse>().ReverseMap();
            CreateMap<Flight, UpdateFlightResponse>().ReverseMap();
            CreateMap<Flight, DeleteFlightResponse>().ReverseMap();

            CreateMap<UpdateFlightCommand, Flight>().ReverseMap();
        }
    }
}
