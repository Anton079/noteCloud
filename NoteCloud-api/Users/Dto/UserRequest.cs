namespace NoteCloud_api.Users.Dto
{
    public class UserRequest
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? Role { get; set; }
        public string Password { get; set; } = default!;
    }
}
