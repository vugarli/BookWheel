
using BookWheel.Api;
using BookWheel.Api.Authorization.Requirements;
using BookWheel.Api.CustomAttribute;
using BookWheel.Api.Filters;
using BookWheel.Application;
using BookWheel.Application.Auth;
using BookWheel.Domain.Services;
using BookWheel.Infrastructure;
using BookWheel.Infrastructure.Identity;
using HybridModelBinding;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger(); ;


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    c.UseDateOnlyTimeOnlyStringConverters();
    c.EnableAnnotations();
    c.OperationFilter<HybridOperationFilter>();
    //c.SchemaFilter<SwaggerExcludeFilter>();
    c.IgnoreObsoleteProperties();
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
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});




// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSerilog();

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

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

builder.Services.AddAuthorization(options=>
{
    options.AddPolicy("CanReserve",p=>p.AddRequirements(new UserCustomerRequirement(),new EmailConfirmedRequirement()));
    options.AddPolicy("CanSetLocation",p => p.AddRequirements(new UserOwnerRequirement(), new EmailConfirmedRequirement()));
    options.AddPolicy("CanRate", p => p.AddRequirements(new UserCustomerRequirement(), new EmailConfirmedRequirement()));
    options.AddPolicy("Owner", p => p.AddRequirements(new UserOwnerRequirement(), new EmailConfirmedRequirement()));
    options.AddPolicy("Customer", p => p.AddRequirements(new UserCustomerRequirement(), new EmailConfirmedRequirement()));
});

builder.Services.AddSingleton<IAuthorizationHandler, UserCustomerRequirementHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, UserOwnerRequirementHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, EmailConfirmedRequirementHandler>();

builder.Services.AddDateOnlyTimeOnlyStringConverters();


builder.Services.AddScoped<OwnerLocationSetter>();

builder.Services.AddControllers(options=>
options.Filters.Add(new ExceptionFilter())
    
    ).AddHybridModelBinder(options =>
{
    /**
     * This is optional and overrides internal ordering of how binding gets applied to a model that doesn't have explicit binding-rules.
     * Internal ordering is: body => form-values => route-values => querystring-values => header-values
     */

    options.FallbackBindingOrder = new[] { Source.Route, Source.Body};
}); ;

var app = builder.Build();

app.MapGet("/health",()=>"Good");


app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
//}


app.UseCors(builder =>
           builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

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

        var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();


        await dbContextData.Database.MigrateAsync();
        await dbContextIdentity.Database.MigrateAsync();
        await ApplicationIdentityDbContextSeed.SeedAsync(dbContextIdentity,roleManager);
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