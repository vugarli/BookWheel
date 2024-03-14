using BookWheel.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        private IConfiguration _configuration { get; }
        private IMediator _mediator { get; }

        public ApplicationDbContext
            (
            IConfiguration configuration,
            IMediator mediator
            )
        {
            _configuration = configuration;
            _mediator = mediator;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("MSSQL"),opt=>opt.UseNetTopologySuite());
            
            base.OnConfiguring(optionsBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            foreach (var history in this.ChangeTracker.Entries()
            .Where(e => typeof(e.Entity) == typeof(BaseEntity<>) && (e.State == EntityState.Added ||
                e.State == EntityState.Modified))
            .Select(e => e.Entity as BaseEntity<>)
            )
            {
                history.ModifiedAt = DateTime.Now;
                if (history.CreatedAt <= DateTime.MinValue)
                {
                    history.CreatedAt = DateTime.Now;
                }
            }

            var result = await base.SaveChangesAsync(cancellationToken);
            
            // dispatch events

           


            return result;
        }
    }
}
