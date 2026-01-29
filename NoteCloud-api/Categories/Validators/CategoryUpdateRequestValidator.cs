using FluentValidation;
using NoteCloud_api.Categories.Dto;

namespace NoteCloud_api.Categories.Validators
{
    public class CategoryUpdateRequestValidator : AbstractValidator<CategoryUpdateRequest>
    {
        public CategoryUpdateRequestValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.Name));
        }
    }
}
