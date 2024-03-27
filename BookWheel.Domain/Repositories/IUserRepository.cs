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

        public Task CreateUserAsync(ApplicationUserRoot user);

    }
}
