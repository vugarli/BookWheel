﻿using BookWheel.Application.Auth;
using BookWheel.Domain;
using BookWheel.Domain.Interfaces;
using BookWheel.Domain.Repositories;
using BookWheel.Infrastructure.Identity;
using BookWheel.Infrastructure.Repositories;
using BookWheel.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace BookWheel.Infrastructure
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>();

            services.AddDbContext<ApplicationIdentityDbContext>()
                .AddIdentity<ApplicationIdentityUser, IdentityRole<Guid>>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>();
            
            services.AddScoped<ITokenService, TokenService>();

            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEmailSender, EmailSender>();

            services.AddCustomIdentity();

            return services;
        }

        public static IServiceCollection AddCustomIdentity(this IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {

                
                // Default Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // Default Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
                
                // Default SignIn settings.
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;

                // Default User settings.
                options.User.AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            return services;
        }

    }
}
