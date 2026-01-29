using FluentValidation;
using NoteCloud_api.Notes.Dto;

namespace NoteCloud_api.Notes.Validators
{
    public class NoteRequestValidator : AbstractValidator<NoteRequest>
    {
        public NoteRequestValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Content).NotEmpty().MaximumLength(5000);
            RuleFor(x => x.CategoryId).NotEmpty();
        }
    }
}
