using NoteCloud_api.System.Exceptions;
using NoteCloud_api.System.ValueObjects;
using NoteCloud_api.Users.ValueObjects;
using NoteCloud_api.Users.Dto;
using NoteCloud_api.Users.Models;

namespace NoteCloud_api.Users.Commands
{
    public sealed class UserUpdateCommand
    {
        public Optional<string> FirstName { get; }
        public Optional<string> LastName { get; }
        public Optional<EmailAddress> Email { get; }
        public Optional<string> Role { get; }
        public Optional<string> Password { get; }

        private UserUpdateCommand(
            Optional<string> firstName,
            Optional<string> lastName,
            Optional<EmailAddress> email,
            Optional<string> role,
            Optional<string> password)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Role = role;
            Password = password;
        }

        public static UserUpdateCommand From(UserUpdateRequest req)
        {
            Optional<string> firstName;
            if (req.FirstName == null)
                firstName = Optional<string>.None();
            else if (string.IsNullOrWhiteSpace(req.FirstName))
                throw new ValidationAppException("FirstName este obligatoriu.");
            else
                firstName = Optional<string>.From(req.FirstName.Trim());

            Optional<string> lastName;
            if (req.LastName == null)
                lastName = Optional<string>.None();
            else if (string.IsNullOrWhiteSpace(req.LastName))
                throw new ValidationAppException("LastName este obligatoriu.");
            else
                lastName = Optional<string>.From(req.LastName.Trim());

            Optional<EmailAddress> email;
            if (req.Email == null)
                email = Optional<EmailAddress>.None();
            else
                email = Optional<EmailAddress>.From(EmailAddress.Create(req.Email));

            Optional<string> role;
            if (req.Role == null)
                role = Optional<string>.None();
            else if (string.IsNullOrWhiteSpace(req.Role))
                throw new ValidationAppException("Role este obligatoriu.");
            else
                role = Optional<string>.From(req.Role.Trim());

            Optional<string> password;
            if (req.Password == null)
                password = Optional<string>.None();
            else if (string.IsNullOrWhiteSpace(req.Password))
                throw new ValidationAppException("Password este obligatoriu.");
            else
                password = Optional<string>.From(req.Password);

            return new UserUpdateCommand(firstName, lastName, email, role, password);
        }

        public void ApplyTo(User user)
        {
            FirstName.Apply(v => user.FirstName = v);
            LastName.Apply(v => user.LastName = v);
            Email.Apply(v => user.Email = v.Value);
            Role.Apply(v => user.Role = v);
        }
    }
}
