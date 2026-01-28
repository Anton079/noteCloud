namespace NoteCloud_api.Auth.Models
{
    public record AuthenticatedUser(string Id, string Username, string DisplayName, string Role);
}
