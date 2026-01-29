using Microsoft.EntityFrameworkCore;
using NoteCloud_api.Data;
using NoteCloud_api.Notes.Models;

namespace NoteCloud_api.Notes.Repository
{
    public class NoteRepo : INoteRepo
    {
        private readonly AppDbContext _db;

        public NoteRepo(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Note> AddAsync(Note note)
        {
            _db.Notes.Add(note);
            await _db.SaveChangesAsync();
            return note;
        }

        public async Task<Note> UpdateAsync(Note note)
        {
            _db.Notes.Update(note);
            await _db.SaveChangesAsync();
            return note;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var note = await _db.Notes.FirstOrDefaultAsync(n => n.Id == id);
            if (note == null)
                return false;

            _db.Notes.Remove(note);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<Note?> GetByIdAsync(Guid id, Guid userId, bool isAdmin)
        {
            var query = _db.Notes.AsQueryable();
            if (!isAdmin)
            {
                query = query.Where(n => n.UserId == userId);
            }

            return await query.FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<List<Note>> GetAllAsync(Guid userId, bool isAdmin)
        {
            var query = _db.Notes.AsQueryable();
            if (!isAdmin)
            {
                query = query.Where(n => n.UserId == userId);
            }

            return await query
                .OrderByDescending(n => n.Date)
                .ToListAsync();
        }

        public async Task<List<Note>> GetByCategoryAsync(Guid categoryId, Guid userId, bool isAdmin)
        {
            var query = _db.Notes
                .Where(n => n.CategoryId == categoryId);

            if (!isAdmin)
            {
                query = query.Where(n => n.UserId == userId);
            }

            return await query
                .OrderByDescending(n => n.Date)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(string title, Guid categoryId, DateTime date, Guid userId)
        {
            return await _db.Notes.AnyAsync(n =>
                n.Title.ToLower() == title.ToLower() &&
                n.CategoryId == categoryId &&
                n.Date == date &&
                n.UserId == userId);
        }
    }
}
