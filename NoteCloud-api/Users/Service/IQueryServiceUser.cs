using NoteCloud_api.Users.Dto;

namespace NoteCloud_api.Users.Service
{
    public interface IQueryServiceUser
    {
        Task<UserResponse> FindUserByIdAsync(string id);
        Task<UserResponse> FindUserByEmailAsync(string email);
        Task<List<UserResponse>> GetAllUsersAsync();
    }
}
