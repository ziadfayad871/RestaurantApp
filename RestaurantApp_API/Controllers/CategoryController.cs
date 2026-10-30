using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantApp_API.DTOs.CategoryDTO;
using RestaurantApp_API.IRepository;
using RestaurantApp_API.Model;

namespace RestaurantApp_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<CategoryDTO>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var categoryDtos = _mapper.Map<IEnumerable<CategoryDTO>>(categories);

            return Ok(new APIResponse<IEnumerable<CategoryDTO>>
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                Data = categoryDtos
            });
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(APIResponse<CategoryDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category is null)
            {
                return NotFound(new APIResponse<object>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Category not found"
                });
            }

            return Ok(new APIResponse<CategoryDTO>
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                Data = _mapper.Map<CategoryDTO>(category)
            });
        }

        [HttpPost]
        [ProducesResponseType(typeof(APIResponse<CategoryDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(APIResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDTO categoryCreateDto)
        {
            var category = _mapper.Map<Category>(categoryCreateDto);
            category.CreatedDate = DateTime.UtcNow;

            await _categoryRepository.CreateAsync(category);
            await _categoryRepository.SaveAsync();

            var categoryDto = _mapper.Map<CategoryDTO>(category);

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, new APIResponse<CategoryDTO>
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status201Created,
                Data = categoryDto
            });
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(APIResponse<CategoryDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryUpdateDTO categoryUpdateDto)
        {
            if (id != categoryUpdateDto.Id)
            {
                return BadRequest(new APIResponse<object>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Category ID mismatch"
                });
            }

            var category = await _categoryRepository.GetByIdAsync(id);

            if (category is null)
            {
                return NotFound(new APIResponse<object>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Category not found"
                });
            }

            _mapper.Map(categoryUpdateDto, category);
            await _categoryRepository.UpdateAsync(category);
            await _categoryRepository.SaveAsync();

            return Ok(new APIResponse<CategoryDTO>
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                Data = _mapper.Map<CategoryDTO>(category)
            });
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(APIResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category is null)
            {
                return NotFound(new APIResponse<object>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Category not found"
                });
            }

            await _categoryRepository.DeleteAsync(category);
            await _categoryRepository.SaveAsync();

            return NoContent();
        }
    }
}
