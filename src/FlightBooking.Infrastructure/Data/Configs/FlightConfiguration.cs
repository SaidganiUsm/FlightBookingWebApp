using FlightBookingApp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace FlightBookingApp.Infrastructure.Data.Configs
{
    public class FlightConfiguration : IEntityTypeConfiguration<Flight>
    {
        public void Configure(EntityTypeBuilder<Flight> builder)
        {
            builder.Property(r => r.DeparturePoint)
                .HasMaxLength(50)
                .IsRequired(true);

            builder.Property(r => r.DestinationPoint)
                .HasMaxLength(50)
                .IsRequired(true);

            builder.HasOne(l => l.DestinationPoint)
                .WithMany(l => l.Flights)
                .HasForeignKey(l => l.DestinationPointId)
                .IsRequired(true);

            builder.HasOne(l => l.DeparturePoint)
                .WithMany(l => l.Flights)
                .HasForeignKey(l =>l.DeparturePointId)
                .IsRequired(true);

            builder.Property(r => r.StartDateTime)
                .HasConversion(v => v, v => DateTime
                .SpecifyKind(v, DateTimeKind.Utc));

            builder.Property(r => r.EndDateTime)
                .HasConversion(v => v, v => DateTime
                .SpecifyKind(v, DateTimeKind.Utc));

            builder.Property(r => r.CreationDate)
                .HasConversion(v => v, v => DateTime
                .SpecifyKind(v, DateTimeKind.Utc));

            builder.Property(r => r.ModificationDate)
                .HasConversion(v => v, v => DateTime
                .SpecifyKind(v, DateTimeKind.Utc));
        }
    }
}
