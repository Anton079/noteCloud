using NoteCloud_api.Users.Models;

namespace NoteCloud_api.Users.Repository
{
    public interface IUserRepo
    {
        Task<User> AddAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<bool> DeleteAsync(Guid id);
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task<List<User>> GetAllAsync();
        Task<bool> EmailExistsAsync(string email);
    }
}
