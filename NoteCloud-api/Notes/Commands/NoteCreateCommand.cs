using NoteCloud_api.Notes.Dto;
using NoteCloud_api.System.Exceptions;
using NoteCloud_api.Notes.ValueObjects;

namespace NoteCloud_api.Notes.Commands
{
    public sealed class NoteCreateCommand
    {
        public NoteTitle Title { get; }
        public NoteContent Content { get; }
        public Guid CategoryId { get; }
        public bool IsFavorite { get; }
        public DateTime Date { get; }

        private NoteCreateCommand(NoteTitle title, NoteContent content, Guid categoryId, bool isFavorite, DateTime date)
        {
            Title = title;
            Content = content;
            CategoryId = categoryId;
            IsFavorite = isFavorite;
            Date = date;
        }

        public static NoteCreateCommand From(NoteRequest req)
        {
            var title = NoteTitle.Create(req.Title);
            var content = NoteContent.Create(req.Content);
            if (req.CategoryId == Guid.Empty)
                throw new ValidationAppException("CategoryId este obligatoriu.");

            var date = req.Date == default ? DateTime.UtcNow : req.Date;
            return new NoteCreateCommand(title, content, req.CategoryId, req.IsFavorite, date);
        }
    }
}
