using NoteCloud_api.System;

namespace NoteCloud_api.Notes.Exceptions
{
    public class NoteNotFoundException : Exception
    {
        public NoteNotFoundException() : base(ExceptionMessage.NoteNotFoundException) { }
    }
}
