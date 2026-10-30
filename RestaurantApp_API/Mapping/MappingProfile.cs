using AutoMapper;
using RestaurantApp_API.DTOs.CategoryDTO;
using RestaurantApp_API.DTOs.MenuItemDTO;
using RestaurantApp_API.Model;

namespace RestaurantApp_API.Mapping
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