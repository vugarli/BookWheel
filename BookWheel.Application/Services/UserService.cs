using BookWheel.Domain.AggregateRoots;
using BookWheel.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Services
{
    public interface IUserService
    {
        public Task<bool> CreateOwnerUserAsync(Guid Id, string email);


    }


    public class UserService : IUserService
    {
        public UserService(IUserRepository userRepository)
        {
            UserRepository = userRepository;
        }

        public IUserRepository UserRepository { get; }

        public async Task<bool> CreateOwnerUserAsync(Guid Id,string email)
        {
            var owner = new OwnerUserRoot(Id,email,email,email);

            await UserRepository.CreateUserAsync(owner);
            
            return true;
        }


    }
}
