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
                new Role { Name = "Admin", },
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
                            var adminRole = roles.Find(r => r.Name == "Admin");
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

            if (!_context.Locations.Any())
            {
                var locations = new List<Location>
                {
                    new Location { 
                        City = "Tashkent", 
                        State = "1", 
                        Country = "Uzbekistan", 
                        Address = "Central Tashkent" 
                    },
                    new Location { 
                        City = "Samarkand", 
                        State = "2", 
                        Country = "Uzbekistan", 
                        Address = "Historical Samarkand" 
                    },
                    new Location { 
                        City = "Bukhara", 
                        State = "3", 
                        Country = "Uzbekistan", 
                        Address = "Old Bukhara" 
                    },
                    new Location { 
                        City = "Khiva", 
                        State = "4", Country = "Uzbekistan", Address = "Ancient Khiva" },
                    new Location { 
                        City = "Andijan", 
                        State = "5", 
                        Country = "Uzbekistan", 
                        Address = "Andijan City Center" 
                    },
                    new Location { 
                        City = "Namangan", 
                        State = "6", 
                        Country = "Uzbekistan", 
                        Address = "Namangan Downtown" 
                    },
                    new Location { 
                        City = "Fergana", 
                        State = "7", 
                        Country = "Uzbekistan", 
                        Address = "Fergana Valley" 
                    },
                    new Location { 
                        City = "Nukus", 
                        State = "8", 
                        Country = "Uzbekistan", 
                        Address = "Nukus City" 
                    },
                    new Location { 
                        City = "Termez", 
                        State = "9", 
                        Country = "Uzbekistan", 
                        Address = "Termez Historical Area" 
                    },
                    new Location { 
                        City = "Kokand", 
                        State = "10", 
                        Country = "Uzbekistan", 
                        Address = "Kokand Center" 
                    }
                };

                _context.Locations.AddRange(locations);
            }

            await _context.SaveChangesAsync();
        }
    }
}
