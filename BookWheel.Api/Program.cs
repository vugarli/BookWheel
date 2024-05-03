
using BookWheel.Application;
using BookWheel.Application.Auth;
using BookWheel.Domain.Services;
using BookWheel.Infrastructure;
using BookWheel.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    c.UseDateOnlyTimeOnlyStringConverters();
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    }

    );
});



// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(AppConstants.JWTKEY)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
}).AddCookie(options =>
{
    options.Events.OnRedirectToAccessDenied =
    options.Events.OnRedirectToLogin = c =>
    {
        c.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.FromResult<object>(null);
    };
});

builder.Services.AddAuthorization();
builder.Services.AddDateOnlyTimeOnlyStringConverters();

builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddApplicationServices();
builder.Services.AddScoped<OwnerLocationSetter>();

builder.Services.AddControllers();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI();


app.UseRouting();

app.MapControllers();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

try
{
    using (var serviceScope = app.Services.CreateScope())
    {
        var dbContextData = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var dbContextIdentity = serviceScope.ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>();

        await dbContextData.Database.MigrateAsync();
        await dbContextIdentity.Database.MigrateAsync();
        // or dbContext.Database.EnsureCreatedAsync();
    }

    // Middleware pipeline configuration

    app.Run();
}
catch (Exception e)
{
    app.Logger.LogCritical(e, "An exception occurred during the service startup");
}
finally
{
    // Flush logs or else you lose very important exception logs.
    // if you use Serilog you can do it via
    // await Log.CloseAndFlushAsync();
}

public partial class Program;