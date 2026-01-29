using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NoteCloud_api.Auth.Models;
using NoteCloud_api.Notes.Dto;
using NoteCloud_api.Notes.Exceptions;
using NoteCloud_api.Notes.Service;

namespace NoteCloud_api.Notes.Controllers
{
    [ApiController]
    [Route("api/v1/notes")]
    public class NotesController : ControllerBase
    {
        private readonly ICommandServiceNote _command;
        private readonly IQueryServiceNote _query;

        public NotesController(ICommandServiceNote command, IQueryServiceNote query)
        {
            _command = command;
            _query = query;
        }

        [HttpGet]
        [Authorize(Policy = "read:note")]
        public async Task<ActionResult<NoteListRequest>> GetAll([FromQuery] string? categoryId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrWhiteSpace(userId))
                    return Unauthorized(new { message = "Invalid user context." });

                var isAdmin = User.IsInRole(SystemRoles.Admin);

                if (!string.IsNullOrWhiteSpace(categoryId))
                {
                    var byCategory = await _query.GetNotesByCategoryAsync(categoryId, userId, isAdmin);
                    return Ok(byCategory);
                }

                var notes = await _query.GetAllNotesAsync(userId, isAdmin);
                return Ok(notes);
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

        [HttpGet("{id}")]
        [Authorize(Policy = "read:note")]
        public async Task<ActionResult<NoteResponse>> GetById(string id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrWhiteSpace(userId))
                    return Unauthorized(new { message = "Invalid user context." });

                var isAdmin = User.IsInRole(SystemRoles.Admin);

                var note = await _query.FindNoteByIdAsync(id, userId, isAdmin);
                return Ok(note);
            }
            catch (NoteNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Internal server error." });
            }
        }

        [HttpPost]
        [Authorize(Policy = "write:note")]
        public async Task<ActionResult<NoteResponse>> Create([FromBody] NoteRequest req)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrWhiteSpace(userId))
                    return Unauthorized(new { message = "Invalid user context." });

                var created = await _command.CreateNote(req, userId);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (NoteAlreadyExistsException ex)
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
        [Authorize(Policy = "write:note")]
        public async Task<ActionResult<NoteResponse>> Update(string id, [FromBody] NoteUpdateRequest req)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrWhiteSpace(userId))
                    return Unauthorized(new { message = "Invalid user context." });

                var isAdmin = User.IsInRole(SystemRoles.Admin);

                var updated = await _command.UpdateNote(id, req, userId, isAdmin);
                return Ok(updated);
            }
            catch (NoteNotFoundException ex)
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

        [HttpDelete("{id}")]
        [Authorize(Policy = "write:note")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrWhiteSpace(userId))
                    return Unauthorized(new { message = "Invalid user context." });

                var isAdmin = User.IsInRole(SystemRoles.Admin);

                await _command.DeleteNote(id, userId, isAdmin);
                return Ok(new { message = "Deleted" });
            }
            catch (NoteNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Internal server error." });
            }
        }
    }
}
