using Xunit;

namespace BookWheel.IntegrationTests.LocationTests;
[Collection("Sequential")]
public class RepositoryGetById : IClassFixture<SharedDatabaseFixture>
{
    public SharedDatabaseFixture Fixture { get; set; }

    public RepositoryGetById(SharedDatabaseFixture fixture)
    {
        Fixture = fixture;
    }
    
    
    
    
    
}