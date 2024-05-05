using Azure;
using BookWheel.Domain.AggregateRoots;
using BookWheel.Domain.LocationAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Hosting;
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
            builder.Property(c=>c.Id).ValueGeneratedNever();

            builder
                .HasOne<CustomerUserRoot>()
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany<Service>(r => r.Services)
                .WithMany()
                .UsingEntity(
                    l => l.HasOne(typeof(Service)).WithMany().OnDelete(DeleteBehavior.NoAction),
                    r => r.HasOne(typeof(Reservation)).WithMany().OnDelete(DeleteBehavior.NoAction));

            builder.OwnsOne(r=>r.PaymentDetails);

            builder.OwnsOne(r=>r.ReservationTimeInterval);


        }
    }
}
