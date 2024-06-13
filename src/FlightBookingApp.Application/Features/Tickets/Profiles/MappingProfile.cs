using AutoMapper;
using FlightBookingApp.Application.Common.DTOs;
using FlightBookingApp.Application.Features.Tickets.Query.GetAll;
using FlightBookingApp.Application.Features.Tickets.Query.GetById;
using FlightBookingApp.Core.Entities;
using FlightBookingApp.Core.Persistence.Paging;

namespace FlightBookingApp.Application.Features.Tickets.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Ticket, GetAllTicketsResponse>().ReverseMap();
            CreateMap<IPaginate<Ticket>, GetListResponseDto<GetAllTicketsResponse>>().ReverseMap();
            CreateMap<Ticket, GetByIdTicketResponse>().ReverseMap();
        }
    }
}
