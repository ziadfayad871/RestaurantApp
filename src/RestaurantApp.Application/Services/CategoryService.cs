using AutoMapper;
using RestaurantApp.Application.Common;
using RestaurantApp.Application.DTOs.Categories;
using RestaurantApp.Application.Interfaces;
using RestaurantApp.Domain.Entities;

namespace RestaurantApp.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<CategoryDTO>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return _mapper.Map<List<CategoryDTO>>(categories);
        }

        public async Task<CategoryDTO?> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return category is null ? null : _mapper.Map<CategoryDTO>(category);
        }

        public async Task<ServiceResult<CategoryDTO>> CreateAsync(CategoryCreateDTO categoryCreateDto)
        {
            var category = _mapper.Map<Category>(categoryCreateDto);
            category.CreatedDate = DateTime.UtcNow;

            await _categoryRepository.CreateAsync(category);
            await _categoryRepository.SaveAsync();

            return ServiceResult<CategoryDTO>.Success(_mapper.Map<CategoryDTO>(category));
        }

        public async Task<ServiceResult<CategoryDTO>> UpdateAsync(int id, CategoryUpdateDTO categoryUpdateDto)
        {
            if (id != categoryUpdateDto.Id)
            {
                return ServiceResult<CategoryDTO>.BadRequest("Category ID mismatch");
            }

            var category = await _categoryRepository.GetByIdAsync(id);

            if (category is null)
            {
                return ServiceResult<CategoryDTO>.NotFound("Category not found");
            }

            _mapper.Map(categoryUpdateDto, category);
            await _categoryRepository.UpdateAsync(category);
            await _categoryRepository.SaveAsync();

            return ServiceResult<CategoryDTO>.Success(_mapper.Map<CategoryDTO>(category));
        }

        public async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category is null)
            {
                return ServiceResult<bool>.NotFound("Category not found");
            }

            await _categoryRepository.DeleteAsync(category);
            await _categoryRepository.SaveAsync();

            return ServiceResult<bool>.Success(true);
        }
    }
}
