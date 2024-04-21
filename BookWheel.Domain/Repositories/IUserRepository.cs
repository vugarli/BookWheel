using BookWheel.Domain.AggregateRoots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Domain.Repositories
{
    public interface IUserRepository
    {

        public Task CreateCustomerAsync(CustomerUserRoot user);
        public Task CreateOwnerAsync(OwnerUserRoot user);

        public Task<CustomerUserRoot?> GetCustomerBySpecificationAsync
        (
            BookWheel.Domain.Specifications.Specification<CustomerUserRoot> spec
        );
    }
}
