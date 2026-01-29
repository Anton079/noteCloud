using FluentValidation;
using NoteCloud_api.Users.Dto;

namespace NoteCloud_api.Users.Validators
{
    public class UserUpdateRequestValidator : AbstractValidator<UserUpdateRequest>
    {
        public UserUpdateRequestValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .When(x => !string.IsNullOrWhiteSpace(x.Email));

            RuleFor(x => x.FirstName)
                .MaximumLength(50)
                .When(x => !string.IsNullOrWhiteSpace(x.FirstName));

            RuleFor(x => x.LastName)
                .MaximumLength(50)
                .When(x => !string.IsNullOrWhiteSpace(x.LastName));

            RuleFor(x => x.Password)
                .MinimumLength(6)
                .When(x => !string.IsNullOrWhiteSpace(x.Password));
        }
    }
}
