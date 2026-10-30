using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantApp_API.DTOs.MenuItemDTO;
using RestaurantApp_API.IRepository;
using RestaurantApp_API.Model;

namespace RestaurantApp_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController : ControllerBase
    {
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public MenuItemController(
            IMenuItemRepository menuItemRepository,
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            _menuItemRepository = menuItemRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<MenuItemDTO>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMenuItems()
        {
            var menuItems = await _menuItemRepository.GetAllAsync();
            var menuItemDtos = _mapper.Map<IEnumerable<MenuItemDTO>>(menuItems);

            return Ok(new APIResponse<IEnumerable<MenuItemDTO>>
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                Data = menuItemDtos
            });
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(APIResponse<MenuItemDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMenuItem(int id)
        {
            var menuItem = await _menuItemRepository.GetByIdAsync(id);

            if (menuItem is null)
            {
                return NotFound(new APIResponse<object>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Menu item not found"
                });
            }

            return Ok(new APIResponse<MenuItemDTO>
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                Data = _mapper.Map<MenuItemDTO>(menuItem)
            });
        }

        [HttpPost]
        [ProducesResponseType(typeof(APIResponse<MenuItemDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(APIResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateMenuItem([FromBody] MenuItemCreateDTO menuItemCreateDto)
        {
            var category = await _categoryRepository.GetByIdAsync(menuItemCreateDto.CategoryId);

            if (category is null)
            {
                return BadRequest(new APIResponse<object>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Invalid CategoryId",
                    ErrorMessages = new List<string>
                    {
                        $"Category with id {menuItemCreateDto.CategoryId} does not exist."
                    }
                });
            }

            var menuItem = _mapper.Map<MenuItem>(menuItemCreateDto);
            menuItem.CreatedDate = DateTime.UtcNow;
            menuItem.Category = category;

            await _menuItemRepository.CreateAsync(menuItem);
            await _menuItemRepository.SaveAsync();

            var menuItemDto = _mapper.Map<MenuItemDTO>(menuItem);

            return CreatedAtAction(nameof(GetMenuItem), new { id = menuItem.Id }, new APIResponse<MenuItemDTO>
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status201Created,
                Data = menuItemDto
            });
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(APIResponse<MenuItemDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateMenuItem(int id, [FromBody] MenuItemUpdateDTO menuItemUpdateDto)
        {
            if (id != menuItemUpdateDto.Id)
            {
                return BadRequest(new APIResponse<object>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Menu item ID mismatch"
                });
            }

            var menuItem = await _menuItemRepository.GetByIdAsync(id);

            if (menuItem is null)
            {
                return NotFound(new APIResponse<object>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Menu item not found"
                });
            }

            var category = await _categoryRepository.GetByIdAsync(menuItemUpdateDto.CategoryId);

            if (category is null)
            {
                return BadRequest(new APIResponse<object>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Invalid CategoryId",
                    ErrorMessages = new List<string>
                    {
                        $"Category with id {menuItemUpdateDto.CategoryId} does not exist."
                    }
                });
            }

            _mapper.Map(menuItemUpdateDto, menuItem);
            menuItem.Category = category;

            await _menuItemRepository.UpdateAsync(menuItem);
            await _menuItemRepository.SaveAsync();

            return Ok(new APIResponse<MenuItemDTO>
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                Data = _mapper.Map<MenuItemDTO>(menuItem)
            });
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(APIResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            var menuItem = await _menuItemRepository.GetByIdAsync(id);

            if (menuItem is null)
            {
                return NotFound(new APIResponse<object>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Menu item not found"
                });
            }

            await _menuItemRepository.DeleteAsync(menuItem);
            await _menuItemRepository.SaveAsync();

            return NoContent();
        }
    }
}
