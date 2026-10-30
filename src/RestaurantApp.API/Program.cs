using Microsoft.AspNetCore.Mvc;
using RestaurantApp.API.Models;
using RestaurantApp.Application;
using RestaurantApp.Infrastructure;
using Scalar.AspNetCore;

namespace RestaurantApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(connectionString);

            builder.Services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var errors = context.ModelState
                            .Where(entry => entry.Value?.Errors.Count > 0)
                            .SelectMany(entry => entry.Value!.Errors)
                            .Select(error => string.IsNullOrWhiteSpace(error.ErrorMessage)
                                ? error.Exception?.Message ?? "Invalid value"
                                : error.ErrorMessage)
                            .ToList();

                        return new BadRequestObjectResult(APIResponse<object>.Failure(
                            StatusCodes.Status400BadRequest,
                            "Validation failed",
                            errors.ToArray()));
                    };
                });

            builder.Services.AddOpenApi();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
