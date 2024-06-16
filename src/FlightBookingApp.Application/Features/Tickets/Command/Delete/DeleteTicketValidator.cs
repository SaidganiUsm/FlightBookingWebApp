using FlightBookingApp.Application.Common.Interfaces.Repositories;
using FlightBookingApp.Application.Common.Interfaces.Services;
using FlightBookingApp.Core.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FlightBookingApp.Application.Features.Tickets.Command.Delete
{
	public class DeleteTicketValidator : AbstractValidator<DeleteTicketCommand>
	{
        private readonly ITicketRepository _ticketRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<User> _userManager;
        public DeleteTicketValidator(
            ITicketRepository ticketRepository,
            ICurrentUserService currentUserService,
            UserManager<User> userManager
        )
        {
            _ticketRepository = ticketRepository;
            _currentUserService = currentUserService;
            _userManager = userManager;

			RuleFor(x => x.Id)
				.MustAsync(
					async (id, cancellationToken) =>
					{
						var ticket = await _ticketRepository.GetAsync(
							predicate: x => x.Id == id,
							cancellationToken: cancellationToken
						);

						return ticket != null;
					}
				)
				.WithMessage("Ticket with this Id does not exist");

			RuleFor(x => x.Id)
				.MustAsync(
					async (id, cancellationToken) =>
					{
						var user = await _userManager.Users.FirstOrDefaultAsync(
							u => u.Email == _currentUserService.UserEmail! && !u.IsDeleted,
							cancellationToken: cancellationToken
						);

						var ticket = await _ticketRepository.GetAsync(
							predicate: x => x.Id == id,
							cancellationToken: cancellationToken
						);

						return ticket!.UserId == user!.Id;
					}
				)
				.WithMessage("You can delete only your own ticket");
		}
    }
}
