using FlightBookingApp.Application.Common.Interfaces.Services;
using FlightBookingApp.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using User = FlightBookingApp.Core.Entities.User;

namespace FlightBookingApp.Application.Features.Users.Command.Delete
{
    public class DeleteUserCommand : IRequest<DeletedUserResponse>
    {

    }

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, DeletedUserResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly RoleManager<Role> _roleManager;

        public DeleteUserCommandHandler(
            UserManager<User> userManager, 
            ICurrentUserService currentUserService, 
            RoleManager<Role> roleManager
        )
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
            _roleManager = roleManager;
        }
        public async Task<DeletedUserResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var currentUserEmail = _currentUserService.UserEmail!;

            var user = await _userManager.Users.FirstOrDefaultAsync(
                u => u.Email == currentUserEmail && !u.IsDeleted,
                cancellationToken: cancellationToken
            );

            if (user == null)
            {
                return new DeletedUserResponse
                {
                    IsDeleted = false,
                    Message = "User does not exist or has already been deleted"
                };
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                await _userManager.RemoveFromRoleAsync(user, role);
            }

            user.IsDeleted = true;
            user.DeletionDate = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return new DeletedUserResponse
            {
                IsDeleted = true,
                Message = "User deleted successfully"
            };
        }
    }
}
