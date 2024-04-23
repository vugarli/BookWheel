using BookWheel.Domain.Specifications.Customer;
using BookWheel.Infrastructure.Repositories;
using BookWheel.UnitTests.Builders;
using Xunit;

namespace BookWheel.IntegrationTests.CustomerTests;

public class RepositoryGetById  : IClassFixture<SharedDatabaseFixture>
{
    public SharedDatabaseFixture Fixture { get; set; }

    public RepositoryGetById(SharedDatabaseFixture fixture)
    {
        Fixture = fixture;
    }

    [Fact]
    public async Task GetCustomerByIdAfterAddingIt()
    {
        using (var transaction = Fixture.DbConnection.BeginTransaction())
        {
            var context = Fixture.CreateContext(transaction);
            var repo = new UserRepository(context);
            var Id = Guid.NewGuid();
            var Name = Guid.NewGuid().ToString();

            var customer = new CustomerBuilder().WithId(Id).WithName(Name).Build();
            
            await repo.CreateCustomerAsync(customer);
            await context.SaveChangesAsync();
                
            var readRepo = new UserRepository(Fixture.CreateContext(transaction));
            var spec = new GetCustomerByIdSpecification(Id);
            var customerFromRepo = await readRepo.GetCustomerBySpecificationAsync(spec);

            Assert.NotNull(customerFromRepo);
            Assert.Equal(Id,customerFromRepo.Id);
            Assert.Equal(Name,customerFromRepo.Name);
        }
    }
    
    
    
}