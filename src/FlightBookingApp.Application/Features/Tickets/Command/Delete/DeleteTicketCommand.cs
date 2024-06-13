using FlightBookingApp.Application.Common.Interfaces.Repositories;
using FlightBookingApp.Application.Common.Interfaces.Services;
using FlightBookingApp.Core.Entities;
using FlightBookingApp.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FlightBookingApp.Application.Features.Tickets.Command.Delete
{
	public class DeleteTicketCommand : IRequest<DeleteTicketResponse>
	{
		public int Id { get; set; }
	}

	public class DeleteTicketCommandHandler
		: IRequestHandler<DeleteTicketCommand, DeleteTicketResponse>
	{
		private readonly ITicketRepository _ticketRepository;
		private readonly ITicketStatusRepository _ticketStatusRepository;
		private readonly ICurrentUserService _currentUserService;
		private readonly UserManager<User> _userManager;

        public DeleteTicketCommandHandler(
			ITicketRepository ticketRepository,
			ITicketStatusRepository ticketStatusRepository,
			ICurrentUserService currentUserService,
			UserManager<User> userManager
		)
        {
            _ticketRepository = ticketRepository;
			_ticketStatusRepository = ticketStatusRepository;
			_currentUserService = currentUserService;
			_userManager = userManager;
        }

        public async Task<DeleteTicketResponse> Handle(DeleteTicketCommand request, CancellationToken cancellationToken)
		{
			var ticket = await _ticketRepository.GetAsync(
				t => t.Id == request.Id,
				cancellationToken: cancellationToken
			);

			var status = await _ticketStatusRepository.GetAsync(
				s => s.Name == TicketStatuses.Cancelled.ToString(),
				cancellationToken: cancellationToken
			);

			ticket!.TicketStatus = status;

			return new DeleteTicketResponse { Id = request.Id };
		}
	}
}
