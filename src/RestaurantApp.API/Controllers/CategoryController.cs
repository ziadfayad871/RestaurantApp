using Microsoft.AspNetCore.Mvc;
using RestaurantApp.API.Models;
using RestaurantApp.Application.DTOs.Categories;
using RestaurantApp.Application.Interfaces;

namespace RestaurantApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ApiControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(APIResponse<IReadOnlyList<CategoryDTO>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(APIResponse<IReadOnlyList<CategoryDTO>>.Success(categories));
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(APIResponse<CategoryDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            if (category is null)
            {
                return NotFound(APIResponse<object>.Failure(
                    StatusCodes.Status404NotFound,
                    "Category not found"));
            }

            return Ok(APIResponse<CategoryDTO>.Success(category));
        }

        [HttpPost]
        [ProducesResponseType(typeof(APIResponse<CategoryDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(APIResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDTO categoryCreateDto)
        {
            var result = await _categoryService.CreateAsync(categoryCreateDto);

            if (!result.IsSuccess)
            {
                return HandleFailure(result);
            }

            return CreatedAtAction(
                nameof(GetCategory),
                new { id = result.Data!.Id },
                APIResponse<CategoryDTO>.Success(result.Data, StatusCodes.Status201Created));
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(APIResponse<CategoryDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryUpdateDTO categoryUpdateDto)
        {
            var result = await _categoryService.UpdateAsync(id, categoryUpdateDto);
            return HandleResult(result);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(APIResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteAsync(id);

            if (!result.IsSuccess)
            {
                return HandleFailure(result);
            }

            return NoContent();
        }
    }
}
