using FlightBookingApp.Application.Common.Interfaces.Repositories;
using FlightBookingApp.Application.Common.Interfaces.Services;
using FlightBookingApp.Core.Entities;
using FlightBookingApp.Core.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FlightBookingApp.Application.Features.Tickets.Command.Update
{
	public class UpdateTicketValidator : AbstractValidator<UpdateTicketCommand>
	{
		private readonly ITicketRepository _ticketRepository;
		private readonly ITicketStatusRepository _ticketStatusRepository;
		private readonly ICurrentUserService _currentUserService;
		private readonly IRankRepository _rankRepository;
		private readonly UserManager<User> _userManager;
		public UpdateTicketValidator(
			ITicketRepository ticketRepository,
			ICurrentUserService currentUserService,
			ITicketStatusRepository ticketStatusRepository,
			IRankRepository rankRepository,
			UserManager<User> userManager
		)
		{
			_ticketRepository = ticketRepository;
			_currentUserService = currentUserService;
			_ticketStatusRepository = ticketStatusRepository;
			_rankRepository = rankRepository;
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

            RuleFor(x => x.TicketRank)
                .MustAsync(
                    async (ticketRank, cancellationToken) =>
                    {
                        var rank = await _rankRepository.GetAsync(
                            predicate: x => x.RankName == ticketRank.ToString(),
                            cancellationToken: cancellationToken
                        );

                        return rank != null;
                    }
                )
                .WithMessage("Rank with this name does not exist");

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
				.WithMessage("You can update only your own ticket");

			RuleFor(x => x.Id)
				.MustAsync(
					async (id, cancellationToken) =>
					{
						var ticket = await _ticketRepository.GetAsync(
							predicate: x => x.Id == id,
							include: x => x.Include(x => x.TicketStatus!),
							cancellationToken: cancellationToken
						);

						var status = await _ticketStatusRepository.GetAsync(
							predicate: x => x.Name == TicketStatuses.Cancelled.ToString(),
							cancellationToken: cancellationToken
						);

						return ticket!.TicketStatus != status;
					}
				)
				.WithMessage("You cannot update cancelled ticket information!");

			RuleFor(x => x.Id)
				.MustAsync(
					async (id, cancellationToken) =>
					{
						var ticket = await _ticketRepository.GetAsync(
							predicate: x => x.Id == id,
                            include: x => x.Include(x => x.TicketStatus!),
                            cancellationToken: cancellationToken
						);

						var status = await _ticketStatusRepository.GetAsync(
							predicate: x => x.Name == TicketStatuses.Used.ToString(),
							cancellationToken: cancellationToken
						);

						return ticket!.TicketStatus != status;
					}
				)
				.WithMessage("You cannot update used ticket information!");
		}
    }
}
