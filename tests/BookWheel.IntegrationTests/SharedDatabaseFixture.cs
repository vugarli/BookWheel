using System.Data.Common;
using BookWheel.Infrastructure;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace BookWheel.IntegrationTests;

public class SharedDatabaseFixture : IDisposable
{
    private static object _lock { get; set; } = new();
    private static bool _databaseInitialized;
    public const string CONNECTION_STRING = "Server=localhost;Database=BookWheel.IntegrationTests;User Id=SA;Password=Vugar2003Vs$;TrustServerCertificate=True";
    public DbConnection DbConnection { get; set; }

    public SharedDatabaseFixture()
    {
        DbConnection = new SqlConnection(CONNECTION_STRING);

        //Seed
        Migrate();
        
        DbConnection.Open();
    }


    private void Migrate()
    {
        lock (_lock)
        {
            if (!_databaseInitialized)
            {
                using (var context = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(CONNECTION_STRING, x => x.UseNetTopologySuite()).Options, new Mock<IMediator>().Object))
                {
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();
                }

                _databaseInitialized = true;
            }
        }
    }

    public ApplicationDbContext CreateContext(DbTransaction transaction = null)
    {
        var context = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(DbConnection,x=>x.UseNetTopologySuite()).Options, new Mock<IMediator>().Object);

        if (transaction != null)
        {
            context.Database.UseTransaction(transaction);
        }
        //lock (_lock)
        //{ 
        //    if(!_databaseInitialized)
        //    {
        //        using var samplecontext = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(DbConnection, x => x.UseNetTopologySuite()).Options, new Mock<IMediator>().Object);
        //        samplecontext.Database.UseTransaction(transaction);
        //        samplecontext.Database.MigrateAsync().GetAwaiter().GetResult();
        //        _databaseInitialized = true;
        //    }
        //}
        return context;
    }
    
    public void Dispose()
    {
        DbConnection.Close();
        DbConnection.Dispose();
    }
}