using BookWheel.Domain.LocationAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace BookWheel.Domain.Repositories
{
    public interface ILocationRepository
    {
        public Task AddLocationAsync(Location location);
        public Task<Location?> GetLocationBySpecificationAsync(Specification<Location> spec);
        public Task<bool> CheckLocationBySpecificationAsync(Specification<Location> spec);
    }
}
