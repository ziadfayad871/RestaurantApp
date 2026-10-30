using System.ComponentModel.DataAnnotations;

namespace RestaurantApp.Application.DTOs.Categories
{
    public class CategoryUpdateDTO
    {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
    }
}
