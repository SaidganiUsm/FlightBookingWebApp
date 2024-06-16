﻿using FlightBookingApp.Core.Entities;
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
                        State = "4",
                        Country = "Uzbekistan", 
                        Address = "Ancient Khiva" 
                    },
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
                await _context.SaveChangesAsync();
            }

            if (!_context.FlightStatuses.Any())
            {
                var flightStatuses = new List<FlightStatus>
                {
                    new FlightStatus { Name = "Scheduled" },
                    new FlightStatus { Name = "Cancelled" },
                    new FlightStatus { Name = "InFlight" },
                    new FlightStatus { Name = "Landed" }
                };

                _context.FlightStatuses.AddRange(flightStatuses);
                await _context.SaveChangesAsync();
            }

            if (!_context.TicketStatuses.Any())
            {
                var ticketStatuses = new List<TicketStatus>
                {
                    new TicketStatus { Name = "Booked" },
                    new TicketStatus { Name = "Cancelled" },
                    new TicketStatus { Name = "Used" }
                };

                _context.TicketStatuses.AddRange(ticketStatuses);
                await _context.SaveChangesAsync();
            }

            if (!_context.Ranks.Any())
            {
                var ranks = new List<Rank>
                {
                    new Rank 
                    { 
                        RankName = "Economy", 
                        RankPriceRatio = 1 
                    },
                    new Rank 
                    { 
                        RankName = "Business", 
                        RankPriceRatio = 2 
                    },
                    new Rank 
                    { 
                        RankName = "FirstClass", 
                        RankPriceRatio = 3 
                    }
                };

                _context.Ranks.AddRange(ranks);
                await _context.SaveChangesAsync();
            }

            if (!_context.Flights.Any())
            {
                var locations = _context.Locations.Take(3).ToList();
                var flightStatus = _context.FlightStatuses.Take(2).ToList();

                var flights = new List<Flight>
                {
                    new Flight 
                    { 
                        StartDateTime = DateTime.Now.AddHours(2), 
                        EndDateTime = DateTime.Now.AddHours(5), 
                        DepartureLocationId = locations[0].Id, 
                        DestinationLocationId = locations[2].Id, 
                        FlightStatusId = flightStatus[0].Id,
                        FirstClassTicketsAmount = 10,
                        BusinessTicketsAmount = 20,
                        EconomyTicketsAmount= 50,
                        TotalTickets = 80,
                        TicketsAvailable = 80,
                    },
                    new Flight 
                    { 
                        StartDateTime = DateTime.Now.AddDays(1), 
                        EndDateTime = DateTime.Now.AddDays(1).AddHours(3), 
                        DepartureLocationId = locations[1].Id, 
                        DestinationLocationId = locations[0].Id, 
                        FlightStatusId = flightStatus[1].Id,
                        FirstClassTicketsAmount = 10,
                        BusinessTicketsAmount = 20,
                        EconomyTicketsAmount= 50,
                        TotalTickets = 80,
                        TicketsAvailable = 80,
                    }
                };

                _context.Flights.AddRange(flights);
                await _context.SaveChangesAsync();
            }

            if (!_context.Tickets.Any())
            {
                var ticketStatuses = _context.TicketStatuses.Take(3).ToList();
                var flights = _context.Flights.Take(2).ToList();
                var ranks = _context.Ranks.Take(3).ToList();

                var tickets = new List<Ticket>
                {
                    new Ticket
                    {
                        Price = 100,
                        FlightId = flights[0].Id,
                        UserId = 2,
                        RankId = ranks[0].Id,
                        TicketStatusId = ticketStatuses[0].Id
                    },
                    new Ticket
                    {
                        Price = 200,
                        FlightId = flights[1].Id,
                        UserId = 2,
                        RankId = ranks[1].Id,
                        TicketStatusId = ticketStatuses[0].Id
                    },
                    new Ticket
                    {
                        Price = 150,
                        FlightId = flights[0].Id,
                        UserId = 2,
                        RankId = ranks[2].Id,
                        TicketStatusId = ticketStatuses[0].Id
                    }
                };

                _context.Tickets.AddRange(tickets);
            }

            await _context.SaveChangesAsync();
        }
    }
}
