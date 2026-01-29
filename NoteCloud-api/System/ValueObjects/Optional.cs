namespace NoteCloud_api.System.ValueObjects
{
    public readonly struct Optional<T>
    {
        public bool HasValue { get; }
        public T Value { get; }

        private Optional(bool hasValue, T value)
        {
            HasValue = hasValue;
            Value = value;
        }

        public static Optional<T> None() => new Optional<T>(false, default!);

        public static Optional<T> From(T value) => new Optional<T>(true, value);

        public void Apply(Action<T> apply)
        {
            if (HasValue)
            {
                apply(Value);
            }
        }
    }
}
