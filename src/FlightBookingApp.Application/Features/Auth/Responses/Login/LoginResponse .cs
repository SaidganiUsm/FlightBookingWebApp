using FlightBookingApp.Application.Features.Auth.Responses.BaseModel;
using FlightBookingApp.Application.Features.Auth.Responses.Token;

namespace FlightBookingApp.Application.Features.Auth.Responses.Login
{
    public class LoginResponse : BaseResponse
    {
        public TokenModel Result { get; set; }
    }
}
