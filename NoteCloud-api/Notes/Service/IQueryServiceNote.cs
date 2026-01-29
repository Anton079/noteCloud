using NoteCloud_api.Notes.Dto;

namespace NoteCloud_api.Notes.Service
{
    public interface IQueryServiceNote
    {
        Task<NoteResponse> FindNoteByIdAsync(Guid id, Guid userId, bool isAdmin);
        Task<NoteListRequest> GetAllNotesAsync(Guid userId, bool isAdmin);
        Task<NoteListRequest> GetNotesByCategoryAsync(Guid categoryId, Guid userId, bool isAdmin);
    }
}
