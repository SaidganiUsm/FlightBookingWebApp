using FlightBookingApp.Application.Features.Flights.Admin.Command.BaseValidator;
using FluentValidation;

namespace FlightBookingApp.Application.Features.Flights.Admin.Command.Update
{
    public class UpdateFlightValidator : AbstractValidator<UpdateFlightCommand>
    {
        public UpdateFlightValidator()
        {
            Include(new BaseFlightCommandsValidator());
        }
    }
}
