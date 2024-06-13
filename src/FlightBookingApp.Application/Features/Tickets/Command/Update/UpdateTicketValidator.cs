using FlightBookingApp.Application.Common.Interfaces.Repositories;
using FlightBookingApp.Application.Common.Interfaces.Services;
using FlightBookingApp.Core.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace FlightBookingApp.Application.Features.Tickets.Command.Update
{
	public class UpdateTicketValidator : AbstractValidator<UpdateTicketCommand>
	{
		private readonly ITicketRepository _ticketRepository;
		private readonly ICurrentUserService _currentUserService;
		private readonly UserManager<User> _userManager;
		public UpdateTicketValidator(
			ITicketRepository ticketRepository,
			ICurrentUserService currentUserService,
			UserManager<User> userManager
		)
		{
			_ticketRepository = ticketRepository;
			_currentUserService = currentUserService;
			_userManager = userManager;

			RuleFor(x => x.FlightId)
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
				.WithMessage("You can update only your own ticket");
		}
    }
}
