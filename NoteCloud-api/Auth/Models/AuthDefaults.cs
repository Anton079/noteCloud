namespace NoteCloud_api.Auth.Models
{
    public class AuthDefaults
    {
        public List<DefaultUser> Users { get; set; } = new();
    }

    public class DefaultUser
    {
        public string Id { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Role { get; set; } = SystemRoles.User;
    }
}
