using FlightBookingApp.Application.Features.Flights.Admin.Command.BaseValidator;
using FluentValidation;

namespace FlightBookingApp.Application.Features.Flights.Admin.Command.Create
{
    public class CreateCommandValidator : AbstractValidator<CreateFlightCommand>
    {
        private readonly int DaysBeforeCreation = 1;
        public CreateCommandValidator() 
        {
            Include(new BaseLotCommandsValidator());

            RuleFor(command => command.StartDateTime)
               .Must(BeBeforeOneDays)
               .WithMessage("Start date must be at least 1 days in advance");
        }

        private bool BeBeforeOneDays(DateTime startDateTime)
        {
            DateTime currentDate = DateTime.Now;
            DateTime twoDaysAhead = currentDate.AddDays(DaysBeforeCreation);

            return startDateTime < twoDaysAhead;
        }
    }
}
