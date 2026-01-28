using AutoMapper;
using NoteCloud_api.System.Id;
using NoteCloud_api.Users.Dto;
using NoteCloud_api.Users.Exceptions;
using NoteCloud_api.Users.Models;
using NoteCloud_api.Users.Repository;
using NoteCloud_api.Auth.Services;


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
            if (string.IsNullOrWhiteSpace(req.FirstName))
                throw new ArgumentException("FirstName este obligatoriu.");

            if (string.IsNullOrWhiteSpace(req.LastName))
                throw new ArgumentException("LastName este obligatoriu.");

            if (string.IsNullOrWhiteSpace(req.Email))
                throw new ArgumentException("Email este obligatoriu.");

            if (string.IsNullOrWhiteSpace(req.Password))
                throw new ArgumentException("Password este obligatoriu.");

            var exists = await _repo.EmailExistsAsync(req.Email);
            if (exists)
                throw new UserAlreadyExistsException();

            var user = _mapper.Map<User>(req);
            user.Id = IdGenerator.New("user");
            user.Role = string.IsNullOrWhiteSpace(req.Role) ? "User" : req.Role;

            var hp = PasswordHasher.HashPassword(req.Password);
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
            if (string.IsNullOrWhiteSpace(req.Id))
                throw new ArgumentException("Id este obligatoriu.");

            var user = await _repo.GetByIdAsync(req.Id);
            if (user == null)
                throw new UserNotFoundException();

            if (!string.IsNullOrWhiteSpace(req.Email) && !string.Equals(req.Email, user.Email, StringComparison.OrdinalIgnoreCase))
            {
                var emailUsed = await _repo.EmailExistsAsync(req.Email);
                if (emailUsed)
                    throw new UserAlreadyExistsException();

                user.Email = req.Email;
            }

            if (!string.IsNullOrWhiteSpace(req.FirstName))
                user.FirstName = req.FirstName;

            if (!string.IsNullOrWhiteSpace(req.LastName))
                user.LastName = req.LastName;

            if (!string.IsNullOrWhiteSpace(req.Role))
                user.Role = req.Role;

            if (!string.IsNullOrWhiteSpace(req.Password))
            {
                var hp = PasswordHasher.HashPassword(req.Password);
                user.PasswordHash = hp.Hash;
                user.PasswordSalt = hp.Salt;
                user.Password = null;
            }

            user.UpdatedAt = DateTime.UtcNow;

            var updated = await _repo.UpdateAsync(user);
            return _mapper.Map<UserResponse>(updated);
        }

        public async Task<bool> DeleteUser(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Id este obligatoriu.");

            var success = await _repo.DeleteAsync(id);
            if (!success)
                throw new UserNotFoundException();

            return true;
        }
    }
}
