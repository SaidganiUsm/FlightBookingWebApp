using FlightBookingApp.Application.Common.Interfaces.Services;
using FlightBookingApp.Core.Entities;
using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FlightBookingApp.Application.Features.Users.Command.Update
{
    public class UpdateUserCommand : IRequest<UpdatedUserResponse>
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? PhoneNumber { get; set; }
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UpdatedUserResponse>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandler(
            ICurrentUserService currentUserService,
            UserManager<User> userManager,
            IMapper mapper
        )
        {
            _currentUserService = currentUserService;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<UpdatedUserResponse> Handle(
            UpdateUserCommand request, 
            CancellationToken cancellationToken
        )
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(
                u => u.Email == _currentUserService.UserEmail! && !u.IsDeleted,
                cancellationToken: cancellationToken
            );

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;

            await _userManager.UpdateAsync(user);

            var response = _mapper.Map<UpdatedUserResponse>(user);

            return response;
        }
    }
}
