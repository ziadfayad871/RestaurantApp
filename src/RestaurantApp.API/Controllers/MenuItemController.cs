using Microsoft.AspNetCore.Mvc;
using RestaurantApp.API.Models;
using RestaurantApp.Application.DTOs.MenuItems;
using RestaurantApp.Application.Interfaces;

namespace RestaurantApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController : ApiControllerBase
    {
        private readonly IMenuItemService _menuItemService;

        public MenuItemController(IMenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(APIResponse<IReadOnlyList<MenuItemDTO>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMenuItems()
        {
            var menuItems = await _menuItemService.GetAllAsync();
            return Ok(APIResponse<IReadOnlyList<MenuItemDTO>>.Success(menuItems));
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(APIResponse<MenuItemDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMenuItem(int id)
        {
            var menuItem = await _menuItemService.GetByIdAsync(id);

            if (menuItem is null)
            {
                return NotFound(APIResponse<object>.Failure(
                    StatusCodes.Status404NotFound,
                    "Menu item not found"));
            }

            return Ok(APIResponse<MenuItemDTO>.Success(menuItem));
        }

        [HttpPost]
        [ProducesResponseType(typeof(APIResponse<MenuItemDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(APIResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateMenuItem([FromBody] MenuItemCreateDTO menuItemCreateDto)
        {
            var result = await _menuItemService.CreateAsync(menuItemCreateDto);

            if (!result.IsSuccess)
            {
                return HandleFailure(result);
            }

            return CreatedAtAction(
                nameof(GetMenuItem),
                new { id = result.Data!.Id },
                APIResponse<MenuItemDTO>.Success(result.Data, StatusCodes.Status201Created));
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(APIResponse<MenuItemDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateMenuItem(int id, [FromBody] MenuItemUpdateDTO menuItemUpdateDto)
        {
            var result = await _menuItemService.UpdateAsync(id, menuItemUpdateDto);
            return HandleResult(result);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(APIResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            var result = await _menuItemService.DeleteAsync(id);

            if (!result.IsSuccess)
            {
                return HandleFailure(result);
            }

            return NoContent();
        }
    }
}
