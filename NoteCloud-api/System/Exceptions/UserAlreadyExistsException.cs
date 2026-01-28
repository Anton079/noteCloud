using NoteCloud_api.System;

namespace NoteCloud_api.Users.Exceptions
{
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException() : base(ExceptionMessage.UserAlreadyExists) { }
    }
}
