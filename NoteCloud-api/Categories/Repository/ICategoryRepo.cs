using NoteCloud_api.Categories.Models;

namespace NoteCloud_api.Categories.Repository
{
    public interface ICategoryRepo
    {
        Task<Category> AddAsync(Category category);
        Task<Category> UpdateAsync(Category category);
        Task<bool> DeleteAsync(string id);
        Task<Category?> GetByIdAsync(string id);
        Task<List<Category>> GetAllAsync();
        Task<bool> NameExistsAsync(string name);
    }
}
