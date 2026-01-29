using NoteCloud_api.Notes.Dto;

namespace NoteCloud_api.Notes.Service
{
    public interface ICommandServiceNote
    {
        Task<NoteResponse> CreateNote(NoteRequest req, Guid userId);
        Task<NoteResponse> UpdateNote(Guid id, NoteUpdateRequest req, Guid userId, bool isAdmin);
        Task<bool> DeleteNote(Guid id, Guid userId, bool isAdmin);
    }
}
