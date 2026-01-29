using NoteCloud_api.Users.Dto;

namespace NoteCloud_api.Users.Service
{
    public interface IQueryServiceUser
    {
        Task<UserResponse> FindUserByIdAsync(Guid id);
        Task<UserResponse> FindUserByEmailAsync(string email);
        Task<UserListRequest> GetAllUsersAsync();
    }
}
