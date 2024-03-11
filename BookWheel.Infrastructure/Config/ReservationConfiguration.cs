using BookWheel.Domain.AggregateRoots;
using BookWheel.Domain.LocationAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Infrastructure.Config
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder
                .HasOne<CustomerUserRoot>()
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.OwnsOne(r=>r.PaymentDetails);

            builder
                .HasOne<Schedule>()
                .WithOne()
                .HasForeignKey<Reservation>(r=>r.ScheduleId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
