using BookWheel.Domain;
using BookWheel.Domain.AggregateRoots;
using BookWheel.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookWheel.Infrastructure.Specification;
using Microsoft.EntityFrameworkCore;

namespace BookWheel.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        public ApplicationDbContext _dbContext { get; }
        public IQueryable<CustomerUserRoot> CustomerQueryable { get => _dbContext.Set<CustomerUserRoot>(); }
        public IQueryable<OwnerUserRoot> OwnerQueryable { get => _dbContext.Set<OwnerUserRoot>(); }
        public UserRepository
            (
            ApplicationDbContext dbContext
            )
        {
            _dbContext = dbContext;
        }

        public async Task CreateOwnerAsync(OwnerUserRoot user)
        {
            await _dbContext.Set<OwnerUserRoot>().AddAsync(user);
        }

        public async Task<CustomerUserRoot?> GetCustomerBySpecificationAsync
        (
            Specification<CustomerUserRoot> spec
        )
            => await CustomerQueryable.ApplySpecification(spec).FirstOrDefaultAsync();
        
        public async Task<OwnerUserRoot?> GetOwnerBySpecificationAsync
        (
            Specification<OwnerUserRoot> spec
        )
            => await OwnerQueryable.ApplySpecification(spec).FirstOrDefaultAsync();
        

        public async Task CreateCustomerAsync(CustomerUserRoot user)
        {
            await _dbContext.Set<CustomerUserRoot>().AddAsync(user);
        }

    }
}
