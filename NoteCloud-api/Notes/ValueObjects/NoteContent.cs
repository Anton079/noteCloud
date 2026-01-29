using NoteCloud_api.System.Exceptions;

namespace NoteCloud_api.Notes.ValueObjects
{
    public readonly record struct NoteContent(string Value)
    {
        public static NoteContent Create(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ValidationAppException("Content este obligatoriu.");

            var value = input.Trim();
            if (value.Length > 5000)
                throw new ValidationAppException("Content prea lung.");

            return new NoteContent(value);
        }
    }
}
