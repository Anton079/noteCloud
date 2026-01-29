namespace NoteCloud_api.System.Exceptions
{
    public class ValidationAppException : AppException
    {
        public ValidationAppException(string message) : base(message, 400, "validation_error") { }
    }
}
