namespace NoteCloud_api.System.ValueObjects
{
    public readonly record struct NoteTitle(string Value)
    {
        public static NoteTitle Create(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new Exceptions.ValidationAppException("Title este obligatoriu.");

            var value = input.Trim();
            if (value.Length > 255)
                throw new Exceptions.ValidationAppException("Title prea lung.");

            return new NoteTitle(value);
        }
    }
}
