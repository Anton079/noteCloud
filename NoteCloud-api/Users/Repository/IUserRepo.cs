namespace NoteCloud_api.Users.Repository
{
    using NoteCloud_api.Users.Models;

    public interface IUserRepo
    {
        Task<User> AddAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<bool> DeleteAsync(string id);

        Task<User?> GetByIdAsync(string id);
        Task<User?> GetByEmailAsync(string email);
        Task<List<User>> GetAllAsync();

        Task<bool> EmailExistsAsync(string email);
    }
}
