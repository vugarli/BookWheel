using BookWheel.Domain.AggregateRoots;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Infrastructure.Config
{
    public class ApplicationUserConfiguration
        : IEntityTypeConfiguration<ApplicationUserRoot>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserRoot> builder)
        {
            builder
                .HasDiscriminator(u => u.UserType)
                .HasValue<OwnerUserRoot>(ApplicationUserType.Owner)
                .HasValue<CustomerUserRoot>(ApplicationUserType.Customer);
        }
    }


    public class OwnerUserConfiguration
        : IEntityTypeConfiguration<OwnerUserRoot>
    {
        public void Configure(EntityTypeBuilder<OwnerUserRoot> builder)
        {
            builder.OwnsOne(ou=>ou.Location);
        }
    }

}
