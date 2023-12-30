using AutoMapper;
using demoAsp2.Models.Category.Module;



namespace demoAsp2.Helpers
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            CreateMap<Category, Models.CategoryDto.Category.Request.CategoryRequestDto>();
            CreateMap<Models.CategoryDto.Category.Request.CategoryRequestDto, Category>();

            CreateMap<Category, Models.CategoryDto.Category.Response.CategoryResponseDto>();

        }

    }
}
