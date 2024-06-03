using FlightBookingApp.Core.Entities;
using FlightBookingApp.Infrastructure.Common.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FlightBookingApp.Infrastructure.Persistence
{
    public class ApplicationDbContextInitializer
    {
        private readonly ILogger<ApplicationDbContextInitializer> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly UsersSeedingData _usersData;

        public ApplicationDbContextInitializer(
            ILogger<ApplicationDbContextInitializer> logger,
            ApplicationDbContext context,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IOptions<UsersSeedingData> usersData
        )
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _usersData = usersData.Value;
        }

        public async Task InitialiseAsync()
        {
            try
            {
                if (_context.Database.IsSqlServer())
                {
                    await _context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initialising the database.");
                throw;
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        public async Task TrySeedAsync()
        {
            // Default roles
            var roles = new List<Role>
            {
                new Role { Name = "Administrator", },
                new Role { Name = "User", },
            };

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role.Name!))
                {
                    await _roleManager.CreateAsync(role);
                }
            }

            // Default users
            var users = new List<User>
            {
                new User
                {
                    UserName = _usersData.Emails!.Admin,
                    Email = _usersData.Emails.Admin,
                    EmailConfirmed = true
                },
                new User
                {
                    UserName = _usersData.Emails.User,
                    Email = _usersData.Emails.User,
                    EmailConfirmed = true
                },
            };

            foreach (var user in users)
            {
                if ((await _userManager.FindByNameAsync(user.UserName!) is null))
                {
                    await _userManager.CreateAsync(user, "Test123!");
                    switch (user.UserName)
                    {
                        case "admin@localhost.com":
                            var adminRole = roles.Find(r => r.Name == "Administrator");
                            if (adminRole != null)
                            {
                                await _userManager.AddToRolesAsync(
                                    user,
                                    new List<string> { adminRole.Name! }
                                );
                            }
                            break;
                        case "user@localhost.com":
                            var userRole = roles.Find(r => r.Name == "User");
                            if (userRole != null)
                            {
                                await _userManager.AddToRolesAsync(
                                    user,
                                    new List<string> { userRole.Name! }
                                );
                            }
                            break;
                    }
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
