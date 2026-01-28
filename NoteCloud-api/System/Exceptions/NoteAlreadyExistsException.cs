using NoteCloud_api.System;

namespace NoteCloud_api.Notes.Exceptions
{
    public class NoteAlreadyExistsException : Exception
    {
        public NoteAlreadyExistsException() : base(ExceptionMessage.NoteAlreadyExistsException) { }
    }
}
