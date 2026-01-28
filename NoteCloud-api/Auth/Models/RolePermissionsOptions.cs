namespace NoteCloud_api.Auth.Models
{
    public class RolePermissionsOptions
    {
        public Dictionary<string, string[]> Permissions { get; set; } = new();
    }
}
