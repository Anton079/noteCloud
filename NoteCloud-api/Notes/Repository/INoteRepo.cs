namespace NoteCloud_api.Notes.Repository
{
    using NoteCloud_api.Notes.Models;

    public interface INoteRepo
    {
        Task<Note> AddAsync(Note note);
        Task<Note> UpdateAsync(Note note);
        Task<bool> DeleteAsync(int id);

        Task<Note?> GetByIdAsync(int id);
        Task<List<Note>> GetAllAsync();
        Task<List<Note>> GetByCategoryAsync(string category);

        Task<bool> ExistsAsync(string title, string category, DateTime date);
    }
}
