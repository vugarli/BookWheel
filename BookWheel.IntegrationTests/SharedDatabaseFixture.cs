using System.Data.Common;
using BookWheel.Infrastructure;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace BookWheel.IntegrationTests;

public class SharedDatabaseFixture : IDisposable
{
    private  object _lock { get; set; }

    public const string CONNECTION_STRING = "Server=localhost;Database=BookWheel.IntegrationTests;User Id=SA;Password=Vugar2003Vs$;TrustServerCertificate=True;";
    public DbConnection DbConnection { get; set; }

    public SharedDatabaseFixture()
    {
        DbConnection = new SqlConnection(CONNECTION_STRING);
        
        //Seed
        
        DbConnection.Open();
    }
    
    public ApplicationDbContext CreateContext(DbTransaction transaction = null)
    {
        var context = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(DbConnection,x=>x.UseNetTopologySuite()).Options, new Mock<IMediator>().Object);

        if (transaction != null)
        {
            context.Database.UseTransaction(transaction);
        }
        return context;
    }
    
    public void Dispose()
    {
        DbConnection.Close();
        DbConnection.Dispose();
    }
}