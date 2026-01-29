using AutoMapper;
using NoteCloud_api.System.Exceptions;
using NoteCloud_api.Users.Commands;
using NoteCloud_api.Users.Dto;
using NoteCloud_api.Users.Models;
using NoteCloud_api.Users.Repository;
using NoteCloud_api.Auth.Services;
using NoteCloud_api.Auth.Models;

namespace NoteCloud_api.Users.Service
{
    public class CommandServiceUser : ICommandServiceUser
    {
        private readonly IUserRepo _repo;
        private readonly IMapper _mapper;

        public CommandServiceUser(IUserRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<UserResponse> CreateUser(UserRequest req)
        {
            var command = UserCreateCommand.From(req);
            var exists = await _repo.EmailExistsAsync(command.Email.Value);
            if (exists)
                throw new ConflictAppException("Email deja folosit.");

            var user = _mapper.Map<User>(req);
            user.Role = command.Role;
            user.Email = command.Email.Value;
            user.FirstName = command.FirstName;
            user.LastName = command.LastName;

            var hp = PasswordHasher.HashPassword(command.Password);
            user.PasswordHash = hp.Hash;
            user.PasswordSalt = hp.Salt;
            user.Password = null;

            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = null;
            user.LastLoginAt = null;
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            var created = await _repo.AddAsync(user);
            return _mapper.Map<UserResponse>(created);
        }

        public async Task<UserResponse> UpdateUser(UserUpdateRequest req)
        {
            if (!req.Id.HasValue || req.Id.Value == Guid.Empty)
                throw new ValidationAppException("Id este obligatoriu.");

            var user = await _repo.GetByIdAsync(req.Id.Value);
            if (user == null)
                throw new NotFoundAppException("User nu a fost gasit.");

            var command = UserUpdateCommand.From(req);

            if (command.Email.HasValue && !string.Equals(command.Email.Value.Value, user.Email, StringComparison.OrdinalIgnoreCase))
            {
                var emailUsed = await _repo.EmailExistsAsync(command.Email.Value.Value);
                if (emailUsed)
                    throw new ConflictAppException("Email deja folosit.");
            }

            if (command.Password.HasValue)
            {
                var hp = PasswordHasher.HashPassword(command.Password.Value);
                user.PasswordHash = hp.Hash;
                user.PasswordSalt = hp.Salt;
                user.Password = null;
            }

            command.ApplyTo(user);
            user.UpdatedAt = DateTime.UtcNow;

            var updated = await _repo.UpdateAsync(user);
            return _mapper.Map<UserResponse>(updated);
        }

        public async Task<UserResponse> UpdateUserRole(Guid id, UserRoleUpdateRequest req)
        {
            if (id == Guid.Empty)
                throw new ValidationAppException("Id este obligatoriu.");

            if (string.IsNullOrWhiteSpace(req.Role))
                throw new ValidationAppException("Role este obligatoriu.");

            var normalizedRole = SystemRoles.Normalize(req.Role);
            if (!string.Equals(normalizedRole, SystemRoles.Admin, StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(normalizedRole, SystemRoles.User, StringComparison.OrdinalIgnoreCase))
            {
                throw new ValidationAppException("Role invalid.");
            }

            var user = await _repo.GetByIdAsync(id);
            if (user == null)
                throw new NotFoundAppException("User nu a fost gasit.");

            user.Role = normalizedRole;
            user.UpdatedAt = DateTime.UtcNow;

            var updated = await _repo.UpdateAsync(user);
            return _mapper.Map<UserResponse>(updated);
        }

        public async Task<bool> DeleteUser(Guid id)
        {
            if (id == Guid.Empty)
                throw new ValidationAppException("Id este obligatoriu.");

            var success = await _repo.DeleteAsync(id);
            if (!success)
                throw new NotFoundAppException("User nu a fost gasit.");

            return true;
        }
    }
}
