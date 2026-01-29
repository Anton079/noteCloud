using FluentValidation;
using NoteCloud_api.Users.Dto;

namespace NoteCloud_api.Users.Validators
{
    public class UserRoleUpdateRequestValidator : AbstractValidator<UserRoleUpdateRequest>
    {
        public UserRoleUpdateRequestValidator()
        {
            RuleFor(x => x.Role).NotEmpty();
        }
    }
}
