using FluentValidation;

namespace FlightBookingApp.Application.Features.Flights.Admin.Command.BaseValidator
{
    public class BaseLotCommandsValidator : AbstractValidator<IFlightCommandsValidator>
    {
        private readonly int flightStartAndEndTimeDifferencesInMinutes = 30;
        public BaseLotCommandsValidator()
        {
            RuleFor(l => l.DepartureLocationId)
                .NotEqual(l => l.DestinationLocationId)
                .WithMessage("Desination cannot be the same as a depature place");

            RuleFor(l => l.EndDateTime)
               .GreaterThan(l => l.StartDateTime.AddMinutes(flightStartAndEndTimeDifferencesInMinutes))
               .WithMessage("The difference between the flight start and end time should be at least 30 minutes");
        }
    }
}
