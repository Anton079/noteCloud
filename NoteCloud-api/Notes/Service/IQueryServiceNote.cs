using NoteCloud_api.Notes.Dto;

namespace NoteCloud_api.Notes.Service
{
    public interface IQueryServiceNote
    {
        Task<NoteResponse> FindNoteByIdAsync(int id);
        Task<List<NoteResponse>> GetAllNotesAsync();
        Task<List<NoteResponse>> GetNotesByCategoryAsync(string category);
    }
}
