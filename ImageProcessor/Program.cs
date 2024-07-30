using ImageProcessor;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Minio;
using System.Text.Json;
using System.Threading.Channels;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<Worker>();
builder.Services.AddScoped<IPresignedUploadUrlGeneratorService,PresignedUploadUrlGeneratorService>();

var endpoint = builder.Configuration.GetSection("MINIO").GetValue<string>("ENDPOINT");
var accessKey = builder.Configuration.GetSection("MINIO").GetValue<string>("ACCESS_KEY");
var secretKey = builder.Configuration.GetSection("MINIO").GetValue<string>("SECRET_KEY");

builder.Services.AddSingleton(Channel.CreateUnbounded<string>());

builder.Services.AddMinio(configureClient => configureClient
            .WithEndpoint(endpoint)
            .WithCredentials(accessKey, secretKey)
            .WithSSL(false)
            .Build(),ServiceLifetime.Transient);


var host = builder.Build();

host.MapGet("/geturl",
    async (IPresignedUploadUrlGeneratorService urlGenerator)
    => await urlGenerator.GenerateUrlAsync(Guid.NewGuid().ToString(),"landingimages"));

host.MapPost("/uploadwebhook",
    async ([FromServices] Channel<string> channel, [FromBody] S3Event s3event)
    => await channel.Writer.WriteAsync(s3event.Key));

host.MapGet("/uploadwebhook",()=> "");



host.Run();
