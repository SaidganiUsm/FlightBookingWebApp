using FlightBookingApp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlightBookingApp.Infrastructure.Data.Configs
{
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasOne(t => t.User)
                .WithMany(t => t.BookedTickets)
                .HasForeignKey(t => t.UserId)
                .IsRequired(true);

            builder.HasOne(t => t.Flight)
                .WithMany(t => t.Tickets)
                .HasForeignKey(t => t.FlightId)
                .IsRequired(true);

            builder.HasOne(l => l.TicketStatus)
                .WithMany(ls => ls.Tickets)
                .HasForeignKey(l => l.TicketStatusId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(l => l.Price).HasColumnType("decimal(28,2)").IsRequired(false);

            builder.Property(r => r.CreationDate).HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
            builder.Property(r => r.ModificationDate).HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
        }
    }
}
