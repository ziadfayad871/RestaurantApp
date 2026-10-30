using RestaurantApp.Application.Common;
using RestaurantApp.Application.DTOs.MenuItems;

namespace RestaurantApp.Application.Interfaces
{
    public interface IMenuItemService
    {
        Task<IReadOnlyList<MenuItemDTO>> GetAllAsync();
        Task<MenuItemDTO?> GetByIdAsync(int id);
        Task<ServiceResult<MenuItemDTO>> CreateAsync(MenuItemCreateDTO menuItemCreateDto);
        Task<ServiceResult<MenuItemDTO>> UpdateAsync(int id, MenuItemUpdateDTO menuItemUpdateDto);
        Task<ServiceResult<bool>> DeleteAsync(int id);
    }
}
