using AutoMapper;
using NoteCloud_api.Notes.Dto;
using NoteCloud_api.Notes.Models;

namespace NoteCloud_api.Notes.Mappers
{
    public class NoteMappingProfile : Profile
    {
        public NoteMappingProfile()
        {
            CreateMap<NoteRequest, Note>()
                .ForMember(dest => dest.isFavorite, opt => opt.MapFrom(src => src.IsFavorite));

            CreateMap<NoteUpdateRequest, Note>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Note, NoteResponse>()
                .ForMember(dest => dest.IsFavorite, opt => opt.MapFrom(src => src.isFavorite));
        }
    }
}
