using RestaurantApp_API.Data;
using RestaurantApp_API.IRepository;
using RestaurantApp_API.Model;

namespace RestaurantApp_API.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
