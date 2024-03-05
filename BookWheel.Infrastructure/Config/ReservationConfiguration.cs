using BookWheel.Domain.Entities;
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
                .HasOne(r => r.User)
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(r => r.Schedule)
                .WithOne(s => s.Reservation)
                .HasForeignKey<Reservation>(r=>r.ScheduleId)
                .OnDelete(DeleteBehavior.NoAction);

            //builder
            //    .HasOne(r => r.CustomerRating)
            //    .WithOne(ra => ra.Reservation)
            //    .OnDelete(DeleteBehavior.NoAction);

            //builder
            //    .HasOne(r => r.OwnerRating)
            //    .WithOne(ra => ra.Reservation)
            //    .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
