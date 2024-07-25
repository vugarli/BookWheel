using BookWheel.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace BookWheel.Infrastructure
{
    public class DbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // IConfigurationRoot configuration = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json")
            //    .Build();

            var context = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer("Server=sqlserver;Database=BookWheel;User Id=SA;Password=Vugar2003Vs$;TrustServerCertificate=True;",x=> x.UseNetTopologySuite()).Options, null);
            
            return context;
        }
    }

    public class IdentityDbContextFactory : IDesignTimeDbContextFactory<ApplicationIdentityDbContext>
    {
        public ApplicationIdentityDbContext CreateDbContext(string[] args)
        {
            //IConfigurationRoot configuration = new ConfigurationBuilder()
            //   .SetBasePath(Directory.GetCurrentDirectory())
            //   .AddJsonFile("appsettings.json")
            //   .Build();
            return new ApplicationIdentityDbContext(new DbContextOptionsBuilder<ApplicationIdentityDbContext>().UseSqlServer("Server=sqlserver;Database=BookWheel;User Id=SA;Password=Vugar2003Vs$;TrustServerCertificate=True;").Options);
        }
    }



}
