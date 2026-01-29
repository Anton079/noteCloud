using FluentValidation;
using NoteCloud_api.Notes.Dto;

namespace NoteCloud_api.Notes.Validators
{
    public class NoteUpdateRequestValidator : AbstractValidator<NoteUpdateRequest>
    {
        public NoteUpdateRequestValidator()
        {
            RuleFor(x => x.Title)
                .MaximumLength(255)
                .When(x => !string.IsNullOrWhiteSpace(x.Title));

            RuleFor(x => x.Content)
                .MaximumLength(5000)
                .When(x => !string.IsNullOrWhiteSpace(x.Content));

            RuleFor(x => x.CategoryId)
                .NotEmpty()
                .When(x => x.CategoryId.HasValue);
        }
    }
}
