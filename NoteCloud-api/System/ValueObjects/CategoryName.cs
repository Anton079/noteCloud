namespace NoteCloud_api.System.ValueObjects
{
    public readonly record struct CategoryName(string Value)
    {
        public static CategoryName Create(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new Exceptions.ValidationAppException("Name este obligatoriu.");

            var value = input.Trim().ToLowerInvariant();
            if (value.Length > 100)
                throw new Exceptions.ValidationAppException("Name prea lung.");

            return new CategoryName(value);
        }
    }
}
