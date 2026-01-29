using NoteCloud_api.System.Exceptions;

namespace NoteCloud_api.Notes.ValueObjects
{
    public readonly record struct NoteTitle(string Value)
    {
        public static NoteTitle Create(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ValidationAppException("Title este obligatoriu.");

            var value = input.Trim();
            if (value.Length > 255)
                throw new ValidationAppException("Title prea lung.");

            return new NoteTitle(value);
        }
    }
}
