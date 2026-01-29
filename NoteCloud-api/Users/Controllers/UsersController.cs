using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteCloud_api.Users.Dto;
using NoteCloud_api.Users.Exceptions;
using NoteCloud_api.Users.Service;

namespace NoteCloud_api.Users.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
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
        [Authorize(Policy = "read:user")]
        public async Task<ActionResult<List<UserResponse>>> GetAll()
        {
            try
            {
                var users = await _query.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Internal server error." });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "read:user")]
        public async Task<ActionResult<UserResponse>> GetById(string id)
        {
            try
            {
                var user = await _query.FindUserByIdAsync(id);
                return Ok(user);
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Internal server error." });
            }
        }

        [HttpGet("by-email/{email}")]
        [Authorize(Policy = "read:user")]
        public async Task<ActionResult<UserResponse>> GetByEmail(string email)
        {
            try
            {
                var user = await _query.FindUserByEmailAsync(email);
                return Ok(user);
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Internal server error." });
            }
        }

        [HttpPost]
        [Authorize(Policy = "write:user")]
        public async Task<ActionResult<UserResponse>> Create([FromBody] UserRequest req)
        {
            try
            {
                var created = await _command.CreateUser(req);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (UserAlreadyExistsException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Internal server error." });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "write:user")]
        public async Task<ActionResult<UserResponse>> Update(string id, [FromBody] UserUpdateRequest req)
        {
            try
            {
                req.Id = id;
                var updated = await _command.UpdateUser(req);
                return Ok(updated);
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UserAlreadyExistsException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Internal server error." });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "write:user")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                await _command.DeleteUser(id);
                return Ok(new { message = "Deleted" });
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
