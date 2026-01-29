using AutoMapper;
using NoteCloud_api.Notes.Dto;
using NoteCloud_api.Notes.Exceptions;
using NoteCloud_api.Notes.Repository;

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

        public async Task<NoteResponse> FindNoteByIdAsync(string id, string userId, bool isAdmin)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("UserId este obligatoriu.");

            var note = await _repo.GetByIdAsync(id, userId, isAdmin);
            if (note == null)
                throw new NoteNotFoundException();

            return _mapper.Map<NoteResponse>(note);
        }

        public async Task<NoteListRequest> GetAllNotesAsync(string userId, bool isAdmin)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("UserId este obligatoriu.");

            var notes = await _repo.GetAllAsync(userId, isAdmin);
            return new NoteListRequest
            {
                Notes = _mapper.Map<List<NoteResponse>>(notes)
            };
        }

        public async Task<NoteListRequest> GetNotesByCategoryAsync(string categoryId, string userId, bool isAdmin)
        {
            if (string.IsNullOrWhiteSpace(categoryId))
                throw new ArgumentException("CategoryId este obligatoriu.");

            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("UserId este obligatoriu.");

            var notes = await _repo.GetByCategoryAsync(categoryId, userId, isAdmin);
            return new NoteListRequest
            {
                Notes = _mapper.Map<List<NoteResponse>>(notes)
            };
        }
    }
}
