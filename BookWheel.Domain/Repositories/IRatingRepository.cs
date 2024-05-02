using BookWheel.Domain.RatingAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Domain.Repositories
{
    public interface IRatingRepository
    {
        public Task AddRatingAsync(RatingRoot location);
        public Task<IList<RatingRoot?>> GetRatingsBySpecificationAsync(Specification<RatingRoot> spec);
        public Task<RatingRoot?> GetRatingBySpecificationAsync(Specification<RatingRoot> spec);
    }
}
