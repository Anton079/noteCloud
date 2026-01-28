namespace NoteCloud_api.System
{
    public class ExceptionMessage
    {
        public const string UserNotFound = "Utilizatorul nu a fost gasit.";
        public const string UserAlreadyExists = "Utilizatorul exista deja.";
        public const string InvalidLogin = "Datele de autentificare sunt invalide.";

        public const string NoteNotFoundException = "Notita nu a fost gasita.";
        public const string NoteAlreadyExistsException = "Notita exista deja.";

        public const string InvalidOperation = "Operatiunea nu este permisa.";
    }
}
