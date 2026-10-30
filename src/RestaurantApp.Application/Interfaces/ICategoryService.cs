using RestaurantApp.Application.Common;
using RestaurantApp.Application.DTOs.Categories;

namespace RestaurantApp.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IReadOnlyList<CategoryDTO>> GetAllAsync();
        Task<CategoryDTO?> GetByIdAsync(int id);
        Task<ServiceResult<CategoryDTO>> CreateAsync(CategoryCreateDTO categoryCreateDto);
        Task<ServiceResult<CategoryDTO>> UpdateAsync(int id, CategoryUpdateDTO categoryUpdateDto);
        Task<ServiceResult<bool>> DeleteAsync(int id);
    }
}
