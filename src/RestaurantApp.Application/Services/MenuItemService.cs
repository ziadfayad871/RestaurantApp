using AutoMapper;
using RestaurantApp.Application.Common;
using RestaurantApp.Application.DTOs.MenuItems;
using RestaurantApp.Application.Interfaces;
using RestaurantApp.Domain.Entities;

namespace RestaurantApp.Application.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public MenuItemService(
            IMenuItemRepository menuItemRepository,
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            _menuItemRepository = menuItemRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<MenuItemDTO>> GetAllAsync()
        {
            var menuItems = await _menuItemRepository.GetAllAsync();
            return _mapper.Map<List<MenuItemDTO>>(menuItems);
        }

        public async Task<MenuItemDTO?> GetByIdAsync(int id)
        {
            var menuItem = await _menuItemRepository.GetByIdAsync(id);
            return menuItem is null ? null : _mapper.Map<MenuItemDTO>(menuItem);
        }

        public async Task<ServiceResult<MenuItemDTO>> CreateAsync(MenuItemCreateDTO menuItemCreateDto)
        {
            var category = await _categoryRepository.GetByIdAsync(menuItemCreateDto.CategoryId);

            if (category is null)
            {
                return ServiceResult<MenuItemDTO>.BadRequest(
                    "Invalid CategoryId",
                    $"Category with id {menuItemCreateDto.CategoryId} does not exist.");
            }

            var menuItem = _mapper.Map<MenuItem>(menuItemCreateDto);
            menuItem.CreatedDate = DateTime.UtcNow;
            menuItem.Category = category;

            await _menuItemRepository.CreateAsync(menuItem);
            await _menuItemRepository.SaveAsync();

            return ServiceResult<MenuItemDTO>.Success(_mapper.Map<MenuItemDTO>(menuItem));
        }

        public async Task<ServiceResult<MenuItemDTO>> UpdateAsync(int id, MenuItemUpdateDTO menuItemUpdateDto)
        {
            if (id != menuItemUpdateDto.Id)
            {
                return ServiceResult<MenuItemDTO>.BadRequest("Menu item ID mismatch");
            }

            var menuItem = await _menuItemRepository.GetByIdAsync(id);

            if (menuItem is null)
            {
                return ServiceResult<MenuItemDTO>.NotFound("Menu item not found");
            }

            var category = await _categoryRepository.GetByIdAsync(menuItemUpdateDto.CategoryId);

            if (category is null)
            {
                return ServiceResult<MenuItemDTO>.BadRequest(
                    "Invalid CategoryId",
                    $"Category with id {menuItemUpdateDto.CategoryId} does not exist.");
            }

            _mapper.Map(menuItemUpdateDto, menuItem);
            menuItem.Category = category;

            await _menuItemRepository.UpdateAsync(menuItem);
            await _menuItemRepository.SaveAsync();

            return ServiceResult<MenuItemDTO>.Success(_mapper.Map<MenuItemDTO>(menuItem));
        }

        public async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            var menuItem = await _menuItemRepository.GetByIdAsync(id);

            if (menuItem is null)
            {
                return ServiceResult<bool>.NotFound("Menu item not found");
            }

            await _menuItemRepository.DeleteAsync(menuItem);
            await _menuItemRepository.SaveAsync();

            return ServiceResult<bool>.Success(true);
        }
    }
}
