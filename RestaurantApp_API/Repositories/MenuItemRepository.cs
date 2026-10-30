using Microsoft.EntityFrameworkCore;
using RestaurantApp_API.Data;
using RestaurantApp_API.IRepository;
using RestaurantApp_API.Model;

namespace RestaurantApp_API.Repositories
{
    public class MenuItemRepository : GenericRepository<MenuItem>, IMenuItemRepository
    {
        public MenuItemRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<MenuItem>> GetAllAsync()
        {
            return await _context.MenuItems
                .Include(m => m.Category)
                .AsNoTracking()
                .ToListAsync();
        }

        public override async Task<MenuItem?> GetByIdAsync(int id)
        {
            return await _context.MenuItems
                .Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
