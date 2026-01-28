using NoteCloud_api.Auth.Models;

namespace NoteCloud_api.Auth.Services
{
    public interface IUserAuthenticator
    {
        Task<AuthenticatedUser?> AuthenticateAsync(string username, string password, CancellationToken cancellationToken = default);
    }
}
