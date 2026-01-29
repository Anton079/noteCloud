namespace NoteCloud_api.System.ValueObjects
{
    public readonly record struct EmailAddress(string Value)
    {
        public static EmailAddress Create(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new Exceptions.ValidationAppException("Email este obligatoriu.");

            var value = input.Trim().ToLowerInvariant();
            if (!value.Contains('@'))
                throw new Exceptions.ValidationAppException("Email invalid.");

            return new EmailAddress(value);
        }
    }
}
