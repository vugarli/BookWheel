using BookWheel.Application.Behaviours;
using BookWheel.Application.Schedules.Commands.Create;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(c =>
            {
                c.RegisterServicesFromAssemblies
                (
                    typeof(CreateScheduleCommand).Assembly
                );

                c.AddOpenBehavior(typeof(ValidationBehaviour<,>));
            });

            services.AddValidatorsFromAssembly(typeof(CreateScheduleCommand).Assembly);

            services.AddAutoMapper(typeof(CreateScheduleCommand).Assembly);


            return services;
        }

    }
}
