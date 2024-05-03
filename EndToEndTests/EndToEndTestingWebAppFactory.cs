using BookWheel.Infrastructure;
using BookWheel.Infrastructure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.MsSql;

namespace EndToEndTests
{
    public class EndToEndTestingWebAppFactory
        : WebApplicationFactory<Program>, IAsyncLifetime
    {

        private readonly MsSqlContainer msSqlContainer  = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-preview-ubuntu-22.04")
            .Build();


        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services=>
            {
                services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));
                services.RemoveAll(typeof(DbContextOptions<ApplicationIdentityDbContext>));
                
                services.AddDbContext<ApplicationDbContext>(c=>c.UseSqlServer(msSqlContainer.GetConnectionString(),s=>s.UseNetTopologySuite()));

                services.AddDbContext<ApplicationIdentityDbContext>(c => c.UseSqlServer(msSqlContainer.GetConnectionString()));
            });
        }



        public async Task InitializeAsync()
        {
            await msSqlContainer.StartAsync();
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await msSqlContainer.StopAsync();
        }
    }
}
