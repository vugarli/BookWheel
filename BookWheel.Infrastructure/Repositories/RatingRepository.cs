using BookWheel.Domain;
using BookWheel.Domain.RatingAggregate;
using BookWheel.Domain.Repositories;
using BookWheel.Infrastructure.Specification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Infrastructure.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        public ApplicationDbContext _dbContext { get; }
        public IQueryable<RatingRoot> Queryable { get => _dbContext.Set<RatingRoot>(); }
        public RatingRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddRatingAsync(RatingRoot rating)
        {
            await _dbContext.AddAsync(rating);
        }

        public async Task<RatingRoot?> GetRatingBySpecificationAsync(Specification<RatingRoot> spec)
        => await Queryable.ApplySpecification(spec).FirstOrDefaultAsync();

        public async Task<IList<RatingRoot?>> GetRatingsBySpecificationAsync(Specification<RatingRoot> spec)
        => await Queryable.ApplySpecification(spec).ToListAsync<RatingRoot?>();
    }
}
