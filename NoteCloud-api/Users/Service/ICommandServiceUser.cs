using NoteCloud_api.Users.Dto;

namespace NoteCloud_api.Users.Service
{
    public interface ICommandServiceUser
    {
        Task<UserResponse> CreateUser(UserRequest req);
        Task<UserResponse> UpdateUser(UserUpdateRequest req);
        Task<bool> DeleteUser(string id);
    }
}
