using FlightBookingApp.Core.Entities;
using FlightBookingApp.Infrastructure.Common;
using FlightBookingApp.Infrastructure.Interceptor;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FlightBookingApp.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<
            User,
            Role,
            int,
            IdentityUserClaim<int>,
            UserRole,
            IdentityUserLogin<int>,
            IdentityRoleClaim<int>,
            IdentityUserToken<int>
        >
    {
        private readonly IMediator? _mediator;
        private readonly AuditableEntitySaveChangesInterceptor? _auditableEntitiesInterceptor;

        public ApplicationDbContext() { }

        public ApplicationDbContext(
            DbContextOptions options,
            AuditableEntitySaveChangesInterceptor auditableEntitiesInterceptor,
            IMediator mediator
        )
            : base(options)
        {
            _auditableEntitiesInterceptor = auditableEntitiesInterceptor;
            _mediator = mediator;
        }

        public ApplicationDbContext(DbContextOptions options)
            : base(options) { }

        public virtual DbSet<Flight> Flights => Set<Flight>();

        public virtual DbSet<Ticket> Tickets => Set<Ticket>();

        public virtual DbSet<Rank> TicketTypes => Set<Rank>();

        protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder
				.Entity<User>()
				.Ignore(u => u.AccessFailedCount)
				.Ignore(u => u.LockoutEnabled)
				.Ignore(u => u.LockoutEnd)
				.Ignore(u => u.TwoFactorEnabled)
				.Ignore(u => u.PhoneNumberConfirmed);

			builder.Entity<User>().ToTable("Users");
			builder.Entity<Role>().ToTable("Roles");
			builder.Entity<UserRole>().ToTable("UserRoles");
			builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
			builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
			builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
			builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");

			builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.AddInterceptors(_auditableEntitiesInterceptor!);
		}

		public override async Task<int> SaveChangesAsync(
			CancellationToken cancellationToken = default
		)
		{
			await _mediator!.DispatchDomainEvents(this);

			return await base.SaveChangesAsync(cancellationToken);
		}
    }
}
