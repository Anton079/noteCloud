using AutoMapper;
using NoteCloud_api.Users.Dto;
using NoteCloud_api.Users.Models;

namespace NoteCloud_api.Users.Mappers
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserRequest, User>();

            CreateMap<UserUpdateRequest, User>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<User, UserResponse>()
                .ForMember(dest => dest.FullName,
                    opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
        }
    }
}
