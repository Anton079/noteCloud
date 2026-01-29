using NoteCloud_api.Notes.Dto;

namespace NoteCloud_api.Notes.Service
{
    public interface IQueryServiceNote
    {
        Task<NoteResponse> FindNoteByIdAsync(string id, string userId, bool isAdmin);
        Task<NoteListRequest> GetAllNotesAsync(string userId, bool isAdmin);
        Task<NoteListRequest> GetNotesByCategoryAsync(string categoryId, string userId, bool isAdmin);
    }
}
