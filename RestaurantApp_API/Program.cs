
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantApp_API.Data;
using RestaurantApp_API.IRepository;
using RestaurantApp_API.Mapping;
using RestaurantApp_API.Model;
using RestaurantApp_API.Repositories;
using Scalar.AspNetCore;

namespace RestaurantApp_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            // add conection string to the services container : 
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile).Assembly);
            builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();




            // Add services to the container.

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

                        return new BadRequestObjectResult(new APIResponse<object>
                        {
                            IsSuccess = false,
                            StatusCode = StatusCodes.Status400BadRequest,
                            Message = "Validation failed",
                            ErrorMessages = errors
                        });
                    };
                });
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
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
