﻿using BookWheel.Domain.AggregateRoots;
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
                .Property(o => o.Version)
                .IsRowVersion();

            builder
                .HasOne<OwnerUserRoot>()
                .WithOne(os => os.Location)
                .HasForeignKey<Location>(c=>c.OwnerId);

            builder
                .HasMany(l => l.Reservations)
                .WithOne()
                .HasForeignKey(r=>r.LocationId);

        }
    }
}
