using FlightBookingApp.Application.Features.Flights.Admin.Command.BaseValidator;
using FluentValidation;

namespace FlightBookingApp.Application.Features.Flights.Admin.Command.Create
{
    public class CreateCommandValidator : AbstractValidator<CreateFlightCommand>
    {
        public CreateCommandValidator() 
        {
            Include(new BaseLotCommandsValidator());
        }
    }
}
