using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NoteCloud_api.Users.Dto;
using NoteCloud_api.Users.Service;
using Swashbuckle.AspNetCore.Annotations;

namespace NoteCloud_api.Users.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    [SwaggerTag("User management and role administration")]
    public class UsersController : ControllerBase
    {
        private readonly ICommandServiceUser _command;
        private readonly IQueryServiceUser _query;

        public UsersController(ICommandServiceUser command, IQueryServiceUser query)
        {
            _command = command;
            _query = query;
        }

        [HttpGet]
        [Authorize(Policy = "read:users")]
        [SwaggerOperation(
            Summary = "List users",
            Description = "Returns all users. Requires `read:users` permission."
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Users list", typeof(UserListRequest))]
        public async Task<ActionResult<UserListRequest>> GetAll()
        {
            var users = await _query.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Policy = "read:users")]
        [SwaggerOperation(
            Summary = "Get user by id",
            Description = "Returns a single user by id. Requires `read:users` permission."
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "User found", typeof(UserResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "User not found")]
        public async Task<ActionResult<UserResponse>> GetById(Guid id)
        {
            var user = await _query.FindUserByIdAsync(id);
            return Ok(user);
        }

        [HttpGet("by-email/{email}")]
        [Authorize(Policy = "read:users")]
        [SwaggerOperation(
            Summary = "Get user by email",
            Description = "Returns a single user by email. Requires `read:users` permission."
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "User found", typeof(UserResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "User not found")]
        public async Task<ActionResult<UserResponse>> GetByEmail(string email)
        {
            var user = await _query.FindUserByEmailAsync(email);
            return Ok(user);
        }

        [HttpPost]
        [Authorize(Policy = "write:users")]
        [SwaggerOperation(
            Summary = "Create user",
            Description = "Creates a user. Requires `write:users` permission."
        )]
        [SwaggerResponse(StatusCodes.Status201Created, "User created", typeof(UserResponse))]
        public async Task<ActionResult<UserResponse>> Create([FromBody] UserRequest req)
        {
            var created = await _command.CreateUser(req);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Policy = "write:users")]
        [SwaggerOperation(
            Summary = "Update user",
            Description = "Updates a user profile. Requires `write:users` permission."
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "User updated", typeof(UserResponse))]
        public async Task<ActionResult<UserResponse>> Update(Guid id, [FromBody] UserUpdateRequest req)
        {
            req.Id = id;
            var updated = await _command.UpdateUser(req);
            return Ok(updated);
        }

        [HttpPut("{id:guid}/role")]
        [Authorize(Policy = "write:users")]
        [SwaggerOperation(
            Summary = "Update user role",
            Description = "Promotes/demotes a user. Allowed roles: `Admin`, `User`. Requires `write:users` permission."
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Role updated", typeof(UserResponse))]
        public async Task<ActionResult<UserResponse>> UpdateRole(Guid id, [FromBody] UserRoleUpdateRequest req)
        {
            var updated = await _command.UpdateUserRole(id, req);
            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Policy = "write:users")]
        [SwaggerOperation(
            Summary = "Delete user",
            Description = "Deletes a user by id. Requires `write:users` permission."
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "User deleted")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _command.DeleteUser(id);
            return Ok(new { message = "Deleted" });
        }
    }
}
