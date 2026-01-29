namespace NoteCloud_api.System.Exceptions
{
    public class ConflictAppException : AppException
    {
        public ConflictAppException(string message) : base(message, 409, "conflict") { }
    }
}
