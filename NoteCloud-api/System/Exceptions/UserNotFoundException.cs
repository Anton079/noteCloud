using NoteCloud_api.System;

namespace NoteCloud_api.Users.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException() : base(ExceptionMessage.UserNotFound) { }
    }
}
