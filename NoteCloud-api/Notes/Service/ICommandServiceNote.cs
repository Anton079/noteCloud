using NoteCloud_api.Notes.Dto;

namespace NoteCloud_api.Notes.Service
{
    public interface ICommandServiceNote
    {
        Task<NoteResponse> CreateNote(NoteRequest req, string userId);
        Task<NoteResponse> UpdateNote(string id, NoteUpdateRequest req, string userId, bool isAdmin);
        Task<bool> DeleteNote(string id, string userId, bool isAdmin);
    }
}
