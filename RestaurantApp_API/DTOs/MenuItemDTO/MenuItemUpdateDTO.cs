using System.ComponentModel.DataAnnotations;

namespace RestaurantApp_API.DTOs.MenuItemDTO
{
    public class MenuItemUpdateDTO
    {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Range(0.01, 1000000)]
        public decimal Price { get; set; }
        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
    }
}
