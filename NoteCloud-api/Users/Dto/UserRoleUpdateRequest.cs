using Swashbuckle.AspNetCore.Annotations;

namespace NoteCloud_api.Users.Dto
{
    public class UserRoleUpdateRequest
    {
        public string Role { get; set; } = default!;
    }
}
