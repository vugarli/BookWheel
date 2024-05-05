using BookWheel.Domain.AggregateRoots;
using BookWheel.Domain.LocationAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Infrastructure.Config
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder
                .Property(o => o.ConcurrencyToken)
                .IsConcurrencyToken();

            builder
                .HasOne<OwnerUserRoot>()
                .WithOne()
                .HasForeignKey<Location>(c=>c.OwnerId);

            builder
                .HasMany(l => l.Services)
                .WithOne()
                .HasForeignKey(s=>s.LocationId);
            
            builder
                .HasMany(l => l.ActiveReservations)
                .WithOne()
                .HasForeignKey(r=>r.LocationId);

            builder.OwnsOne(l => l.WorkingTimeRange);

        }
    }
}
