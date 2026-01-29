using NoteCloud_api.System.Exceptions;

namespace NoteCloud_api.Categories.ValueObjects
{
    public readonly record struct CategoryName(string Value)
    {
        public static CategoryName Create(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ValidationAppException("Name este obligatoriu.");

            var value = input.Trim().ToLowerInvariant();
            if (value.Length > 100)
                throw new ValidationAppException("Name prea lung.");

            return new CategoryName(value);
        }
    }
}
