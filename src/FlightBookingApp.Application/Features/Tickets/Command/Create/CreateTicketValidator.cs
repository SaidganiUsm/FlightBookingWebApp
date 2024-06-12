using FluentValidation;

namespace FlightBookingApp.Application.Features.Tickets.Command.Create
{
    public class CreateTicketValidator : AbstractValidator<CreateTicketCommand>
    {
        public CreateTicketValidator()
        {
            
        }
    }
}
