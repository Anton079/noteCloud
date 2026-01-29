using FluentValidation;
using NoteCloud_api.Categories.Dto;

namespace NoteCloud_api.Categories.Validators
{
    public class CategoryRequestValidator : AbstractValidator<CategoryRequest>
    {
        public CategoryRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        }
    }
}
