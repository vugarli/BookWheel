using BookWheel.Domain;
using BookWheel.Domain.Entities;
using BookWheel.Domain.RatingAggregate;
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

        DbSet<RatingRoot> Ratings { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IMediator mediator)
            : base(options)
        {
            _mediator = mediator;
        }

        
        //public ApplicationDbContext
        //   (
        //   IConfiguration configuration
        //   )
        //{
        //    _configuration = configuration;
        //}

        //public ApplicationDbContext
        //    (
        //    IConfiguration configuration,
        //    IMediator mediator
        //    )
        //{
        //    _configuration = configuration;
        //    _mediator = mediator;
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var events = new List<BaseDomainEvent>();
            //var ents = ChangeTracker.Entries().Select(c=>c.Entity is IBaseEntity).ToList();
            var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is IBaseEntity && (e.State == EntityState.Added ||
                e.State == EntityState.Modified))
            .Select(e => e.Entity as IBaseEntity).ToList();

            foreach (var history in entries)
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
