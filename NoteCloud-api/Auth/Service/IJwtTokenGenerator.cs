using System.Security.Claims;
using NoteCloud_api.Auth.Models;

namespace NoteCloud_api.Auth.Services
{
    public interface IJwtTokenGenerator
    {
        GeneratedJwt GenerateToken(AuthenticatedUser user, IEnumerable<Claim>? customClaims = null);
    }
}
