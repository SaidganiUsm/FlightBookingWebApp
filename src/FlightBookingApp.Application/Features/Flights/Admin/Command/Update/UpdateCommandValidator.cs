using FlightBookingApp.Application.Features.Flights.Admin.Command.BaseValidator;
using FluentValidation;

namespace FlightBookingApp.Application.Features.Flights.Admin.Command.Update
{
    public class UpdateCommandValidator : AbstractValidator<UpdateFlightCommand>
    {
        public UpdateCommandValidator()
        {
            Include(new BaseLotCommandsValidator());
        }
    }
}
