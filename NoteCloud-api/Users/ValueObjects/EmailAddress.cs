using NoteCloud_api.System.Exceptions;

namespace NoteCloud_api.Users.ValueObjects
{
    public readonly record struct EmailAddress(string Value)
    {
        public static EmailAddress Create(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ValidationAppException("Email este obligatoriu.");

            var value = input.Trim().ToLowerInvariant();
            if (!value.Contains('@'))
                throw new ValidationAppException("Email invalid.");

            return new EmailAddress(value);
        }
    }
}
