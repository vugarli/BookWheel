using BookWheel.Domain;
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

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IMediator mediator)
            : base(options)
        {
            _mediator = mediator;
        }

        
        public ApplicationDbContext
           (
           IConfiguration configuration
           )
        {
            _configuration = configuration;
        }

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

        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     optionsBuilder.UseSqlServer(_configuration.GetConnectionString("MSSQL"),opt=>opt.UseNetTopologySuite());
        //     base.OnConfiguring(optionsBuilder);
        // }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var events = new List<BaseDomainEvent>();
            foreach (var history in this.ChangeTracker.Entries()
            .Where(e => e.Entity.GetType() == typeof(BaseEntity<>) && (e.State == EntityState.Added ||
                e.State == EntityState.Modified))
            .Select(e => e.Entity as IBaseEntity)
            )
            {
                history.ModifiedAt = DateTime.Now;
                if (history.CreatedAt <= DateTime.MinValue)
                {
                    history.CreatedAt = DateTime.Now;
                }
                events.AddRange(history.Events);
            }

            var result = await base.SaveChangesAsync(cancellationToken);
            
            // dispatch events

            foreach(var domainEvent in events)
                await _mediator.Publish(domainEvent);

            return result;
        }
    }
}
