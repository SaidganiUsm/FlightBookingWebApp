﻿namespace FlightBookingApp.Application.Features.Auth.Responses.BaseModel
{
    public class BaseResponse
    {
        public string? Message { get; set; }

        public bool IsSuccess { get; set; }

        public IEnumerable<string>? Errors { get; set; }
    }
}
