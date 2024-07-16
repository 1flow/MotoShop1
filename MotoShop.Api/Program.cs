using MotoShop.Application.Services;
using MotoShop.DAL.DependencyInjection;
using MotoShop.Domain.Interfaces.Services;
using MotoShop.Application.DependencyInjection;
using Serilog;
using FonTech.Domain.Settings;
using Microsoft.AspNetCore.Diagnostics;
using MotoShop.Api.Middlewares;
using MotoShop.Domain.Settings;
using MotoShop.Producer.DependencyInjection;
using MotoShop.Consumer.DependencyInjection;
using Prometheus;
namespace MotoShop.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthenticationAndAuthorization(builder);
        builder.Services.AddControllers();
        builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.DefaultSection));       
        builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection(nameof(RabbitMqSettings)));       
        builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection(nameof(RedisSettings)));       
        builder.Services.AddSwagger();
        builder.Services.AddDataAccessLayer(builder.Configuration);       
        builder.Services.AddApplication(builder.Configuration);
        builder.Services.AddProducer();
        builder.Services.AddConsumer();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.UseHttpClientMetrics();

        builder.Host.UseSerilog((context, configuration)=>configuration.ReadFrom.Configuration(context.Configuration));

        builder.Configuration.AddUserSecrets<Program>();

        var app = builder.Build();

        app.UseMiddleware<ExceptionHandlingMiddleware>();
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fontech Swagger v1.0");                
                c.RoutePrefix = string.Empty;

            });
        }
        app.UseMetricServer();
        app.UseHttpMetrics();

        app.MapGet("/randon-number", () =>
        {
            var number = Random.Shared.Next(0,10);
            return Results.Ok(number);
        });

        app.UseCors(x=> x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapMetrics();

        app.MapControllers();

        app.Run();
    }
}
