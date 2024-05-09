using BookWheel.Application.Behaviours;
using BookWheel.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookWheel.Application.Reservations.Commands;

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
                    typeof(CreateReservationCommand).Assembly
                );

                c.AddOpenBehavior(typeof(ValidationBehaviour<,>));
            });


            services.AddScoped<GetUserEmailAddressService>();

            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddValidatorsFromAssembly(typeof(CreateReservationCommand).Assembly);

            services.AddAutoMapper(typeof(CreateReservationCommand).Assembly);

            return services;
        }

    }
}
