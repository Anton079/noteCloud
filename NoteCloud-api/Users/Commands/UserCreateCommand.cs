using NoteCloud_api.System.Exceptions;
using NoteCloud_api.Users.ValueObjects;
using NoteCloud_api.Users.Dto;

namespace NoteCloud_api.Users.Commands
{
    public sealed class UserCreateCommand
    {
        public string FirstName { get; }
        public string LastName { get; }
        public EmailAddress Email { get; }
        public string Password { get; }
        public string Role { get; }

        private UserCreateCommand(string firstName, string lastName, EmailAddress email, string password, string role)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            Role = role;
        }

        public static UserCreateCommand From(UserRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.FirstName))
                throw new ValidationAppException("FirstName este obligatoriu.");

            if (string.IsNullOrWhiteSpace(req.LastName))
                throw new ValidationAppException("LastName este obligatoriu.");

            if (string.IsNullOrWhiteSpace(req.Password))
                throw new ValidationAppException("Password este obligatoriu.");

            var email = EmailAddress.Create(req.Email);
            var role = string.IsNullOrWhiteSpace(req.Role) ? "User" : req.Role.Trim();

            return new UserCreateCommand(req.FirstName.Trim(), req.LastName.Trim(), email, req.Password, role);
        }
    }
}
