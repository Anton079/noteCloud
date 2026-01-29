namespace NoteCloud_api.System.ValueObjects
{
    public readonly record struct NoteContent(string Value)
    {
        public static NoteContent Create(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new Exceptions.ValidationAppException("Content este obligatoriu.");

            var value = input.Trim();
            if (value.Length > 5000)
                throw new Exceptions.ValidationAppException("Content prea lung.");

            return new NoteContent(value);
        }
    }
}
