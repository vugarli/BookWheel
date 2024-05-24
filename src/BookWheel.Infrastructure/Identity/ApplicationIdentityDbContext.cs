using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Infrastructure.Identity
{
    public class ApplicationIdentityDbContext 
        : IdentityDbContext
        <
            ApplicationIdentityUser,
            IdentityRole<Guid>,
            Guid
        >
    {

        private IConfiguration _configuration { get; }
        //public ApplicationIdentityDbContext(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //}

        public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options)
            : base(options)
        {
            
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var connectionString = _configuration.GetConnectionString(name: "MSSQL");
        //    optionsBuilder.UseSqlServer(connectionString);
        //    base.OnConfiguring(optionsBuilder);
        //}


        protected override void OnModelCreating(ModelBuilder builder)
        {
            // override defaults
            base.OnModelCreating(builder);
        }

    }
}
