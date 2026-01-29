namespace NoteCloud_api.System.Exceptions
{
    public class NotFoundAppException : AppException
    {
        public NotFoundAppException(string message) : base(message, 404, "not_found") { }
    }
}
