namespace NoteCloud_api.System.Exceptions
{
    public class UnauthorizedAppException : AppException
    {
        public UnauthorizedAppException(string message) : base(message, 401, "unauthorized") { }
    }
}
