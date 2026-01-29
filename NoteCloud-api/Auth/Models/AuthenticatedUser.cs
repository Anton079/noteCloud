namespace NoteCloud_api.Auth.Models
{
    public record AuthenticatedUser(Guid Id, string Username, string DisplayName, string Role);
}
