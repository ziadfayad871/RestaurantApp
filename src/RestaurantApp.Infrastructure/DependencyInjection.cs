using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestaurantApp.Application.Interfaces;
using RestaurantApp.Infrastructure.Persistence;
using RestaurantApp.Infrastructure.Persistence.Repositories;

namespace RestaurantApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IMenuItemRepository, MenuItemRepository>();

            return services;
        }
    }
}
