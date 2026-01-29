using Microsoft.EntityFrameworkCore;
using NoteCloud_api.Categories.Models;
using NoteCloud_api.Data;

namespace NoteCloud_api.Categories.Repository
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly AppDbContext _db;

        public CategoryRepo(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Category> AddAsync(Category category)
        {
            _db.Categories.Add(category);
            await _db.SaveChangesAsync();
            return category;
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            _db.Categories.Update(category);
            await _db.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
                return false;

            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<Category?> GetByIdAsync(string id)
        {
            return await _db.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _db.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<bool> NameExistsAsync(string name)
        {
            return await _db.Categories.AnyAsync(c => c.Name == name);
        }
    }
}
