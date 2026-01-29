using Swashbuckle.AspNetCore.Annotations;

namespace NoteCloud_api.Auth.Dto
{
    public class RegisterRequest
    {
        [SwaggerSchema(Description = "Example: Ana")]
        public string FirstName { get; set; } = string.Empty;

        [SwaggerSchema(Description = "Example: Popescu")]
        public string LastName { get; set; } = string.Empty;

        [SwaggerSchema(Description = "Example: ana@notecloud.local")]
        public string Email { get; set; } = string.Empty;

        [SwaggerSchema(Description = "Example: User123!")]
        public string Password { get; set; } = string.Empty;
    }
}
