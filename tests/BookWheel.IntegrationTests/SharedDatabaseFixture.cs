using System.Data.Common;
using BookWheel.Infrastructure;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Testcontainers.MsSql;
using Xunit;

namespace BookWheel.IntegrationTests;

public class SharedDatabaseFixture : IDisposable, IAsyncLifetime
{
    private static object _lock { get; set; } = new();
    private bool _databaseInitialized;
    public string CONNECTION_STRING;
    public DbConnection DbConnection { get; set; }

    private readonly MsSqlContainer msSqlContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-preview-ubuntu-22.04")
            .WithPassword("Vugar2003Vs$")
            .Build();

    public SqlConnection Connection 
    { get => new SqlConnection(msSqlContainer.GetConnectionString()); }


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

    public async Task InitializeAsync()
    {
        await msSqlContainer.StartAsync();
        var b = new SqlConnectionStringBuilder(msSqlContainer.GetConnectionString());
        b.InitialCatalog = "BookWheel.IntegrationTests";
        CONNECTION_STRING= b.ToString();
        DbConnection = new SqlConnection(CONNECTION_STRING);

        //Seed
        Migrate();

        DbConnection.Open();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await msSqlContainer.StopAsync();
    }
}