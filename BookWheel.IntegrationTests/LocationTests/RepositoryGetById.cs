using Xunit;

namespace BookWheel.IntegrationTests.LocationTests;

public class RepositoryGetById : IClassFixture<SharedDatabaseFixture>
{
    public SharedDatabaseFixture Fixture { get; set; }

    public RepositoryGetById(SharedDatabaseFixture fixture)
    {
        Fixture = fixture;
    }
    
    
    
    
    
}