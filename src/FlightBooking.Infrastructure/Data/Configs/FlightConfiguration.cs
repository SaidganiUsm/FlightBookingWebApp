using FlightBookingApp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace FlightBookingApp.Infrastructure.Data.Configs
{
    public class FlightConfiguration : IEntityTypeConfiguration<Flight>
    {
        public void Configure(EntityTypeBuilder<Flight> builder)
        {
            builder.HasOne(f => f.DepartureLocation)
                .WithMany()
                .HasForeignKey(f => f.DepartureLocationId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(f => f.DestinationLocation)
                .WithMany()
                .HasForeignKey(f => f.DestinationLocationId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

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
