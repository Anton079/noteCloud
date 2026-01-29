using AutoMapper;
using NoteCloud_api.Notes.Commands;
using NoteCloud_api.Notes.Dto;
using NoteCloud_api.Notes.Models;
using NoteCloud_api.Notes.Repository;
using NoteCloud_api.System.Exceptions;

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

        public async Task<NoteResponse> CreateNote(NoteRequest req, Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ValidationAppException("UserId este obligatoriu.");

            var command = NoteCreateCommand.From(req);

            var exists = await _repo.ExistsAsync(command.Title.Value, command.CategoryId, command.Date, userId);
            if (exists)
                throw new ConflictAppException("Note exista deja.");

            var note = _mapper.Map<Note>(req);
            note.Title = command.Title.Value;
            note.Content = command.Content.Value;
            note.CategoryId = command.CategoryId;
            note.Date = command.Date;
            note.isFavorite = command.IsFavorite;
            note.UserId = userId;

            var created = await _repo.AddAsync(note);
            return _mapper.Map<NoteResponse>(created);
        }

        public async Task<NoteResponse> UpdateNote(Guid id, NoteUpdateRequest req, Guid userId, bool isAdmin)
        {
            if (userId == Guid.Empty)
                throw new ValidationAppException("UserId este obligatoriu.");

            var note = await _repo.GetByIdAsync(id, userId, isAdmin);
            if (note == null)
                throw new NotFoundAppException("Note nu a fost gasita.");

            var command = NoteUpdateCommand.From(req);
            command.ApplyTo(note);

            var updated = await _repo.UpdateAsync(note);
            return _mapper.Map<NoteResponse>(updated);
        }

        public async Task<bool> DeleteNote(Guid id, Guid userId, bool isAdmin)
        {
            if (userId == Guid.Empty)
                throw new ValidationAppException("UserId este obligatoriu.");

            var note = await _repo.GetByIdAsync(id, userId, isAdmin);
            if (note == null)
                throw new NotFoundAppException("Note nu a fost gasita.");

            var success = await _repo.DeleteAsync(id);
            if (!success)
                throw new NotFoundAppException("Note nu a fost gasita.");

            return true;
        }
    }
}
