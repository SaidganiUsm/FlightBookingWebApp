using FlightBookingApp.Application.Common.Interfaces.Repositories;
using FluentValidation;

namespace FlightBookingApp.Application.Features.Tickets.Command.Create
{
    public class CreateTicketValidator : AbstractValidator<CreateTicketCommand>
    {
		private readonly int BookingClosingInHours = -2;
        private readonly IFlightRepository _flightRepository;
		private readonly IRankRepository _rankRepository;

        public CreateTicketValidator(
			IFlightRepository flightRepository, 
			IRankRepository rankRepository
		)
        {
            _flightRepository = flightRepository;
			_rankRepository = rankRepository;

			RuleFor(x => x.FlightId)
				.MustAsync(
					async (id, cancellationToken) =>
					{
						var flight = await _flightRepository.GetAsync(
							predicate: x => x.Id == id,
							cancellationToken: cancellationToken
						);

						return flight != null;
					}
				)
				.WithMessage("Flight with this Id does not exist");

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

            RuleFor(x => x.FlightId)
                .MustAsync(
                    async (id, cancellationToken) =>
                    {
                        var flight = await _flightRepository.GetAsync(
						   predicate: x => x.Id == id,
						   cancellationToken: cancellationToken
						);

                        if (flight == null) return false;

                        return DateTime.UtcNow < flight.StartDateTime.AddHours(BookingClosingInHours);
                    }
                )
                .WithMessage("Ticket booking is closed two hours before the flight");
        }
    }
}
