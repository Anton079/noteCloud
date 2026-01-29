namespace NoteCloud_api.Notes.Repository
{
    using NoteCloud_api.Notes.Models;

    public interface INoteRepo
    {
        Task<Note> AddAsync(Note note);
        Task<Note> UpdateAsync(Note note);
        Task<bool> DeleteAsync(Guid id);

        Task<Note?> GetByIdAsync(Guid id, Guid userId, bool isAdmin);
        Task<List<Note>> GetAllAsync(Guid userId, bool isAdmin);
        Task<List<Note>> GetByCategoryAsync(Guid categoryId, Guid userId, bool isAdmin);

        Task<bool> ExistsAsync(string title, Guid categoryId, DateTime date, Guid userId);
    }
}
