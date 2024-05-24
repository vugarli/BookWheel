using BookWheel.Domain.AggregateRoots;
using BookWheel.Domain.Specifications.Owner;
using BookWheel.Infrastructure;
using BookWheel.Infrastructure.Repositories;
using BookWheel.UnitTests.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Xunit;

namespace BookWheel.IntegrationTests.OwnerTests;



public class RepositoryGetById : IClassFixture<SharedDatabaseFixture>
{

    public SharedDatabaseFixture Fixture { get; set; }

    public RepositoryGetById(SharedDatabaseFixture sharedDatabaseFixture)
    {
        Fixture = sharedDatabaseFixture;
    }

    [Fact]
    public async Task GetOwnerByIdAfterAddingIt()
    {
            using var transaction = Fixture.DbConnection.BeginTransaction();
            var wContext =  Fixture.CreateContext(transaction);
            var wRepo = new UserRepository(wContext);

            var Id = Guid.NewGuid();
            var Name = Guid.NewGuid().ToString();

            // var wOwner = OwnerProvider.GetOwner(Id,Name,Name);
            var wOwner = new OwnerBuilder().WithId(Id).WithName(Name).Build();
            await wRepo.CreateOwnerAsync(wOwner);

            await wContext.SaveChangesAsync();
        
            var rContext =  Fixture.CreateContext(transaction);
            var rRepo = new UserRepository(rContext);

            
            var spec = new GetOwnerByIdSpecification(Id);
            var rOwner = await rRepo.GetOwnerBySpecificationAsync(spec);
            
            Assert.NotNull(rOwner);
            Assert.Equal(rOwner.Id,Id);
            Assert.Equal(rOwner.Name,Name);
        
        
        
    }



}