using RestaurantApp.Infrastructure.Persistence;
using RestaurantApp.Application.Interfaces;
using RestaurantApp.Domain.Entities;

namespace RestaurantApp.Infrastructure.Persistence.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
