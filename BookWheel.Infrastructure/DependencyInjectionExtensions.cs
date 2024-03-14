using BookWheel.Domain;
using BookWheel.Domain.Repositories;
using BookWheel.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Infrastructure
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();



            return services;
        }

    }
}
