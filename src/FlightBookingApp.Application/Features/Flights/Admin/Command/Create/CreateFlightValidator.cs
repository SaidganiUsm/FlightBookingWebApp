using FlightBookingApp.Application.Features.Flights.Admin.Command.BaseValidator;
using FluentValidation;

namespace FlightBookingApp.Application.Features.Flights.Admin.Command.Create
{
    public class CreateFlightValidator : AbstractValidator<CreateFlightCommand>
    {
        private readonly int DaysBeforeCreation = 1;
        public CreateFlightValidator() 
        {
            Include(new BaseFlightCommandsValidator());

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
