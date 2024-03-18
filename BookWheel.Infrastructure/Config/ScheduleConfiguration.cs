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
    public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {
            builder
                .Property(o => o.Version)
                .IsRowVersion();

            builder.OwnsOne(c => c.ScheduleTimeRange);

            builder.OwnsOne(s=>s.SchedulePrice);
        }
    }
}
