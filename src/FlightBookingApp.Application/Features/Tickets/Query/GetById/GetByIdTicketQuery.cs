using FlightBookingApp.Application.Common.Interfaces.Repositories;
using MediatR;

namespace FlightBookingApp.Application.Features.Tickets.Query.GetById
{
	public class GetByIdTicketQuery : IRequest<GetByIdTicketResponse>
	{
		public int Id { get; set; }
	}

	public class GetByIdTicketQueryHandler
		: IRequestHandler<GetByIdTicketQuery, GetByIdTicketResponse>
	{
		private readonly ITicketRepository _ticketRepository;

        public GetByIdTicketQueryHandler(ITicketRepository ticketRepository)
        {
			_ticketRepository = ticketRepository;
        }

        public async Task<GetByIdTicketResponse> Handle(
			GetByIdTicketQuery request, 
			CancellationToken cancellationToken
		)
		{
			throw new NotImplementedException();
		}
	}
}
