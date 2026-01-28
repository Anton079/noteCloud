using NoteCloud_api.System;

namespace NoteCloud_api.System.Exceptions
{
    public class InvalidOperationExceptionNoteCloud : Exception
    {
        public InvalidOperationExceptionNoteCloud() : base(ExceptionMessage.InvalidOperation) { }
    }
}
