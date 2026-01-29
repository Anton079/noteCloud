using AutoMapper;
using NoteCloud_api.Notes.Dto;
using NoteCloud_api.Notes.Repository;
using NoteCloud_api.System.Exceptions;

namespace NoteCloud_api.Notes.Service
{
    public class QueryServiceNote : IQueryServiceNote
    {
        private readonly INoteRepo _repo;
        private readonly IMapper _mapper;

        public QueryServiceNote(INoteRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<NoteResponse> FindNoteByIdAsync(Guid id, Guid userId, bool isAdmin)
        {
            if (userId == Guid.Empty)
                throw new ValidationAppException("UserId este obligatoriu.");

            var note = await _repo.GetByIdAsync(id, userId, isAdmin);
            if (note == null)
                throw new NotFoundAppException("Note nu a fost gasita.");

            return _mapper.Map<NoteResponse>(note);
        }

        public async Task<NoteListRequest> GetAllNotesAsync(Guid userId, bool isAdmin)
        {
            if (userId == Guid.Empty)
                throw new ValidationAppException("UserId este obligatoriu.");

            var notes = await _repo.GetAllAsync(userId, isAdmin);
            return new NoteListRequest
            {
                Notes = _mapper.Map<List<NoteResponse>>(notes)
            };
        }

        public async Task<NoteListRequest> GetNotesByCategoryAsync(Guid categoryId, Guid userId, bool isAdmin)
        {
            if (categoryId == Guid.Empty)
                throw new ValidationAppException("CategoryId este obligatoriu.");

            if (userId == Guid.Empty)
                throw new ValidationAppException("UserId este obligatoriu.");

            var notes = await _repo.GetByCategoryAsync(categoryId, userId, isAdmin);
            return new NoteListRequest
            {
                Notes = _mapper.Map<List<NoteResponse>>(notes)
            };
        }
    }
}
