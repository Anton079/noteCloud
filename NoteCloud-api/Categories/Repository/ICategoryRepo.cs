using NoteCloud_api.Categories.Models;

namespace NoteCloud_api.Categories.Repository
{
    public interface ICategoryRepo
    {
        Task<Category> AddAsync(Category category);
        Task<Category> UpdateAsync(Category category);
        Task<bool> DeleteAsync(Guid id);
        Task<Category?> GetByIdAsync(Guid id);
        Task<List<Category>> GetAllAsync();
        Task<bool> NameExistsAsync(string name);
    }
}
