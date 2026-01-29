using AutoMapper;
using NoteCloud_api.Categories.Dto;
using NoteCloud_api.Categories.Models;

namespace NoteCloud_api.Categories.Mappers
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<CategoryRequest, Category>();
            CreateMap<CategoryUpdateRequest, Category>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Category, CategoryResponse>();
        }
    }
}
