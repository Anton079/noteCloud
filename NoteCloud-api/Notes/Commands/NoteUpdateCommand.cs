using NoteCloud_api.Notes.Dto;
using NoteCloud_api.Notes.Models;
using NoteCloud_api.System.Exceptions;
using NoteCloud_api.Notes.ValueObjects;
using NoteCloud_api.System.ValueObjects;

namespace NoteCloud_api.Notes.Commands
{
    public sealed class NoteUpdateCommand
    {
        public Optional<NoteTitle> Title { get; }
        public Optional<NoteContent> Content { get; }
        public Optional<Guid> CategoryId { get; }
        public Optional<bool> IsFavorite { get; }
        public Optional<DateTime> Date { get; }

        private NoteUpdateCommand(
            Optional<NoteTitle> title,
            Optional<NoteContent> content,
            Optional<Guid> categoryId,
            Optional<bool> isFavorite,
            Optional<DateTime> date)
        {
            Title = title;
            Content = content;
            CategoryId = categoryId;
            IsFavorite = isFavorite;
            Date = date;
        }

        public static NoteUpdateCommand From(NoteUpdateRequest req)
        {
            var title = string.IsNullOrWhiteSpace(req.Title)
                ? Optional<NoteTitle>.None()
                : Optional<NoteTitle>.From(NoteTitle.Create(req.Title));

            var content = string.IsNullOrWhiteSpace(req.Content)
                ? Optional<NoteContent>.None()
                : Optional<NoteContent>.From(NoteContent.Create(req.Content));

            Optional<Guid> categoryId;
            if (req.CategoryId == null)
            {
                categoryId = Optional<Guid>.None();
            }
            else if (req.CategoryId == Guid.Empty)
            {
                throw new ValidationAppException("CategoryId este obligatoriu.");
            }
            else
            {
                categoryId = Optional<Guid>.From(req.CategoryId.Value);
            }

            var isFavorite = req.IsFavorite.HasValue
                ? Optional<bool>.From(req.IsFavorite.Value)
                : Optional<bool>.None();

            var date = req.Date.HasValue
                ? Optional<DateTime>.From(req.Date.Value)
                : Optional<DateTime>.None();

            return new NoteUpdateCommand(title, content, categoryId, isFavorite, date);
        }

        public void ApplyTo(Note note)
        {
            Title.Apply(v => note.Title = v.Value);
            Content.Apply(v => note.Content = v.Value);
            CategoryId.Apply(v => note.CategoryId = v);
            IsFavorite.Apply(v => note.isFavorite = v);
            Date.Apply(v => note.Date = v);
        }
    }
}
