using NoteCloud_api.System;

namespace NoteCloud_api.System.Exceptions
{
    public class InvalidLoginException : Exception
    {
        public InvalidLoginException() : base(ExceptionMessage.InvalidLogin) { }
    }
}
