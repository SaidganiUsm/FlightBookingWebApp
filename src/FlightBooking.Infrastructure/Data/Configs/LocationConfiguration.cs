using FlightBookingApp.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FlightBookingApp.Infrastructure.Data.Configs
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.Property(l => l.Country)
                .HasMaxLength(50)
                .IsRequired(true);

            builder.Property(l => l.State)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(l => l.City)
                .HasMaxLength(50)
                .IsRequired(true);

            builder.Property(l => l.Address)
                .HasMaxLength(150)
                .IsRequired(true);
        }
    }
}
