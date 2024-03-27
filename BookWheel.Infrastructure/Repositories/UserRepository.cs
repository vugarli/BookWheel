using BookWheel.Domain;
using BookWheel.Domain.AggregateRoots;
using BookWheel.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        public ApplicationDbContext _dbContext { get; }
        public UserRepository
            (
            ApplicationDbContext dbContext,
            IUnitOfWork unitOfWork
            )
        {
            _dbContext = dbContext;
        }


        public async Task CreateUserAsync(ApplicationUserRoot user)
        {
            await _dbContext.Set<ApplicationUserRoot>().AddAsync(user);
        }

    }
}
