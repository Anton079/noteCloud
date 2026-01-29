using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NoteCloud_api.Auth.Models;
using NoteCloud_api.Data;


namespace NoteCloud_api.Auth.Services
{
    public class UserAuthenticator : IUserAuthenticator
    {
        private readonly AppDbContext? _dbContext;
        private readonly IReadOnlyDictionary<string, DefaultUser> _defaults;
        private readonly ILogger<UserAuthenticator> _logger;

        public UserAuthenticator(AppDbContext? dbContext, IOptions<AuthDefaults> defaults, ILogger<UserAuthenticator> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
            _defaults = BuildDefaults(defaults?.Value);
        }

        public async Task<AuthenticatedUser?> AuthenticateAsync(string username, string password, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            var normalizedUsername = username.Trim().ToLowerInvariant();

            if (_dbContext != null)
            {
                var user = await TryAuthenticateUser(normalizedUsername, password, cancellationToken);
                if (user != null)
                {
                    return user;
                }
            }

            if (_defaults.TryGetValue(normalizedUsername, out var defaultUser))
            {
                if (string.Equals(defaultUser.Password, password, StringComparison.Ordinal))
                {
                    var role = string.IsNullOrWhiteSpace(defaultUser.Role) ? SystemRoles.User : SystemRoles.Normalize(defaultUser.Role);
                    var id = Guid.TryParse(defaultUser.Id, out var parsed) ? parsed : Guid.NewGuid();
                    return new AuthenticatedUser(id, defaultUser.Username, defaultUser.DisplayName, role);
                }
            }

            return null;
        }

        private async Task<AuthenticatedUser?> TryAuthenticateUser(string username, string password, CancellationToken cancellationToken)
        {
            if (_dbContext == null)
            {
                return null;
            }

            var user = await _dbContext.Users.AsNoTracking()
                .Where(u => u.Email == username)
                .Select(u => new
                {
                    u.Id,
                    u.Email,
                    u.FirstName,
                    u.LastName,
                    u.Role,
                    u.PasswordHash,
                    u.PasswordSalt
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (user != null && !string.IsNullOrEmpty(user.PasswordHash) && !string.IsNullOrEmpty(user.PasswordSalt))
            {
                if (PasswordHasher.Verify(password, user.PasswordHash, user.PasswordSalt))
                {
                    var role = string.IsNullOrWhiteSpace(user.Role) ? SystemRoles.User : SystemRoles.Normalize(user.Role);

                    var displayName = $"{user.FirstName} {user.LastName}".Trim();
                    if (string.IsNullOrWhiteSpace(displayName))
                    {
                        displayName = user.Email;
                    }

                    return new AuthenticatedUser(user.Id, user.Email!, displayName, role);
                }

                _logger.LogInformation("Failed password verification for user {Username}", username);
            }

            return null;
        }

        private static IReadOnlyDictionary<string, DefaultUser> BuildDefaults(AuthDefaults? defaults)
        {
            if (defaults?.Users != null && defaults.Users.Count > 0)
            {
                return defaults.Users
                    .Where(u => !string.IsNullOrWhiteSpace(u.Username) && !string.IsNullOrWhiteSpace(u.Password))
                    .ToDictionary(u => u.Username, u => u, StringComparer.OrdinalIgnoreCase);
            }

            return new Dictionary<string, DefaultUser>(StringComparer.OrdinalIgnoreCase);
        }
    }
}
