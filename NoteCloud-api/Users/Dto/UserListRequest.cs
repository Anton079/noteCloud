namespace NoteCloud_api.Users.Dto
{
    public class UserListRequest
    {
        public List<UserResponse> Users { get; set; } = new();
    }
}
