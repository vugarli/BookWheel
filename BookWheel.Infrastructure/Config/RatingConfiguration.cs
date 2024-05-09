using BookWheel.Domain.LocationAggregate;
using BookWheel.Domain.RatingAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Infrastructure.Config
{
    public class RatingConfiguration
        : IEntityTypeConfiguration<RatingRoot>
    {

        public void Configure(EntityTypeBuilder<RatingRoot> builder)
        {
            
        }
    }
}
