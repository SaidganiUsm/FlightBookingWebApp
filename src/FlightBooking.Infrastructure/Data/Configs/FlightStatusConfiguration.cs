using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using FlightBookingApp.Core.Entities;

namespace FlightBookingApp.Infrastructure.Data.Configs
{
    public class FlightStatusConfiguration : IEntityTypeConfiguration<FlightStatus>
    {
        public void Configure(EntityTypeBuilder<FlightStatus> builder)
        {
            builder.Property(s => s.Name).HasMaxLength(50).IsRequired(true);
        }
    }
}
