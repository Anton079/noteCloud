using NoteCloud_api.Notes.Dto;

namespace NoteCloud_api.Notes.Service
{
    public interface IQueryServiceNote
    {
        Task<NoteResponse> FindNoteByIdAsync(int id, string userId, bool isAdmin);
        Task<List<NoteResponse>> GetAllNotesAsync(string userId, bool isAdmin);
        Task<List<NoteResponse>> GetNotesByCategoryAsync(string category, string userId, bool isAdmin);
    }
}
