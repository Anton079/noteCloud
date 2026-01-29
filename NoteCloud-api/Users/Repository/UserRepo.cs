using Microsoft.EntityFrameworkCore;
using NoteCloud_api.Data;
using NoteCloud_api.Users.Models;

namespace NoteCloud_api.Users.Repository
{
    public class UserRepo : IUserRepo
    {
        private readonly AppDbContext _db;

        public UserRepo(AppDbContext db)
        {
            _db = db;
        }

        public async Task<User> AddAsync(User user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return false;

            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _db.Users
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _db.Users.AnyAsync(u => u.Email == email);
        }
    }
}
