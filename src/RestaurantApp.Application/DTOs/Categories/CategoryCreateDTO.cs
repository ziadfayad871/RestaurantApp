using System.ComponentModel.DataAnnotations;

namespace RestaurantApp.Application.DTOs.Categories
{
    public class CategoryCreateDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
    }
}
