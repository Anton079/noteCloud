namespace NoteCloud_api.Notes.Repository
{
    using NoteCloud_api.Notes.Models;

    public interface INoteRepo
    {
        Task<Note> AddAsync(Note note);
        Task<Note> UpdateAsync(Note note);
        Task<bool> DeleteAsync(string id);

        Task<Note?> GetByIdAsync(string id, string userId, bool isAdmin);
        Task<List<Note>> GetAllAsync(string userId, bool isAdmin);
        Task<List<Note>> GetByCategoryAsync(string categoryId, string userId, bool isAdmin);

        Task<bool> ExistsAsync(string title, string categoryId, DateTime date, string userId);
    }
}
