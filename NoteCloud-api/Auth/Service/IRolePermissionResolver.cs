namespace NoteCloud_api.Auth.Services
{
    public interface IRolePermissionResolver
    {
        IReadOnlyCollection<string> GetPermissions(string role);
    }
}
