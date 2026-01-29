using AutoMapper;
using NoteCloud_api.Notes.Dto;
using NoteCloud_api.Notes.Exceptions;
using NoteCloud_api.Notes.Models;
using NoteCloud_api.Notes.Repository;
using NoteCloud_api.System.Id;

namespace NoteCloud_api.Notes.Service
{
    public class CommandServiceNote : ICommandServiceNote
    {
        private readonly INoteRepo _repo;
        private readonly IMapper _mapper;

        public CommandServiceNote(INoteRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<NoteResponse> CreateNote(NoteRequest req, string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("UserId este obligatoriu.");

            if (string.IsNullOrWhiteSpace(req.Title))
                throw new ArgumentException("Title este obligatoriu.");

            if (string.IsNullOrWhiteSpace(req.Content))
                throw new ArgumentException("Content este obligatoriu.");

            if (string.IsNullOrWhiteSpace(req.CategoryId))
                throw new ArgumentException("CategoryId este obligatoriu.");

            var date = req.Date == default ? DateTime.UtcNow : req.Date;

            var exists = await _repo.ExistsAsync(req.Title, req.CategoryId, date, userId);
            if (exists)
                throw new NoteAlreadyExistsException();

            var note = _mapper.Map<Note>(req);
            note.Id = IdGenerator.New("note");
            note.Date = date;
            note.isFavorite = req.IsFavorite;
            note.UserId = userId;

            var created = await _repo.AddAsync(note);
            return _mapper.Map<NoteResponse>(created);
        }

        public async Task<NoteResponse> UpdateNote(string id, NoteUpdateRequest req, string userId, bool isAdmin)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("UserId este obligatoriu.");

            var note = await _repo.GetByIdAsync(id, userId, isAdmin);
            if (note == null)
                throw new NoteNotFoundException();

            if (!string.IsNullOrWhiteSpace(req.Title))
                note.Title = req.Title;

            if (!string.IsNullOrWhiteSpace(req.Content))
                note.Content = req.Content;

            if (!string.IsNullOrWhiteSpace(req.CategoryId))
                note.CategoryId = req.CategoryId;

            if (req.IsFavorite.HasValue)
                note.isFavorite = req.IsFavorite.Value;

            if (req.Date.HasValue)
                note.Date = req.Date.Value;

            var updated = await _repo.UpdateAsync(note);
            return _mapper.Map<NoteResponse>(updated);
        }

        public async Task<bool> DeleteNote(string id, string userId, bool isAdmin)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("UserId este obligatoriu.");

            var note = await _repo.GetByIdAsync(id, userId, isAdmin);
            if (note == null)
                throw new NoteNotFoundException();

            var success = await _repo.DeleteAsync(id);
            if (!success)
                throw new NoteNotFoundException();

            return true;
        }
    }
}
