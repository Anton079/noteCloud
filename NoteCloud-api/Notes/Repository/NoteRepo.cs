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

        public async Task<bool> DeleteAsync(int id)
        {
            var note = await _db.Notes.FirstOrDefaultAsync(n => n.Id == id);
            if (note == null)
                return false;

            _db.Notes.Remove(note);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<Note?> GetByIdAsync(int id)
        {
            return await _db.Notes.FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<List<Note>> GetAllAsync()
        {
            return await _db.Notes
                .OrderByDescending(n => n.Date)
                .ToListAsync();
        }

        public async Task<List<Note>> GetByCategoryAsync(string category)
        {
            return await _db.Notes
                .Where(n => n.Category.ToLower() == category.ToLower())
                .OrderByDescending(n => n.Date)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(string title, string category, DateTime date)
        {
            return await _db.Notes.AnyAsync(n =>
                n.Title.ToLower() == title.ToLower() &&
                n.Category.ToLower() == category.ToLower() &&
                n.Date == date);
        }
    }
}
