using BookWheel.Domain.LocationAggregate;
using BookWheel.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Domain.Repositories
{
    public interface ILocationRepository
    {
        public Task<Location?> GetLocationBySpecificationAsync(Specification<Location> spec);
        

    }
}
