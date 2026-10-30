using AutoMapper;
using RestaurantApp.Application.DTOs.Categories;
using RestaurantApp.Application.DTOs.MenuItems;
using RestaurantApp.Domain.Entities;

namespace RestaurantApp.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryCreateDTO, Category>();
            CreateMap<CategoryUpdateDTO, Category>();

            CreateMap<MenuItem, MenuItemDTO>()
                .ForMember(dest => dest.CategoryName,
                           opt => opt.MapFrom(src => src.Category != null
                                                      ? src.Category.Name
                                                      : string.Empty));

            CreateMap<MenuItemCreateDTO, MenuItem>();
            CreateMap<MenuItemUpdateDTO, MenuItem>();
        }
    }
}