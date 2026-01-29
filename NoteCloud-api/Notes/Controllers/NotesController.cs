using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NoteCloud_api.Auth.Models;
using NoteCloud_api.Notes.Dto;
using NoteCloud_api.Notes.Service;
using NoteCloud_api.System.Exceptions;
using Swashbuckle.AspNetCore.Annotations;

namespace NoteCloud_api.Notes.Controllers
{
    [ApiController]
    [Route("api/v1/notes")]
    [SwaggerTag("Notes CRUD, favorites, and category filtering")]
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
        [SwaggerOperation(
            Summary = "List notes",
            Description = "Returns notes visible to the current user.\n\n- **Admin** sees all notes.\n- **User** sees only their own notes.\n- Use `categoryId` to filter by category.",
            OperationId = "Notes_GetAll")]
        [SwaggerResponse(StatusCodes.Status200OK, "Notes list", typeof(NoteListRequest))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Validation failed")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Authentication required")]
        public async Task<ActionResult<NoteListRequest>> GetAll([FromQuery] Guid? categoryId)
        {
            var userIdRaw = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdRaw, out var userId) || userId == Guid.Empty)
                throw new ValidationAppException("UserId este obligatoriu.");

            var isAdmin = User.IsInRole(SystemRoles.Admin);

            if (categoryId.HasValue)
            {
                var byCategory = await _query.GetNotesByCategoryAsync(categoryId.Value, userId, isAdmin);
                return Ok(byCategory);
            }

            var notes = await _query.GetAllNotesAsync(userId, isAdmin);
            return Ok(notes);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Policy = "read:note")]
        [SwaggerOperation(
            Summary = "Get note by id",
            Description = "Returns a single note by id if the current user is allowed to read it.",
            OperationId = "Notes_GetById")]
        [SwaggerResponse(StatusCodes.Status200OK, "Note found", typeof(NoteResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Authentication required")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Note not found")]
        public async Task<ActionResult<NoteResponse>> GetById(Guid id)
        {
            var userIdRaw = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdRaw, out var userId) || userId == Guid.Empty)
                throw new ValidationAppException("UserId este obligatoriu.");

            var isAdmin = User.IsInRole(SystemRoles.Admin);

            var note = await _query.FindNoteByIdAsync(id, userId, isAdmin);
            return Ok(note);
        }

        [HttpPost]
        [Authorize(Policy = "write:note")]
        [SwaggerOperation(
            Summary = "Create note",
            Description = "Creates a note owned by the current user.",
            OperationId = "Notes_Create")]
        [SwaggerResponse(StatusCodes.Status201Created, "Note created", typeof(NoteResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Validation failed")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Authentication required")]
        public async Task<ActionResult<NoteResponse>> Create([FromBody] NoteRequest req)
        {
            var userIdRaw = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdRaw, out var userId) || userId == Guid.Empty)
                throw new ValidationAppException("UserId este obligatoriu.");

            var created = await _command.CreateNote(req, userId);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }


        [HttpPut("{id:guid}")]
        [Authorize(Policy = "write:note")]
        [SwaggerOperation(
            Summary = "Update note",
            Description = "Updates fields for a note owned by the current user. Admin can update any note.",
            OperationId = "Notes_Update")]
        [SwaggerResponse(StatusCodes.Status200OK, "Note updated", typeof(NoteResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Validation failed")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Authentication required")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Note not found")]
        public async Task<ActionResult<NoteResponse>> Update(Guid id, [FromBody] NoteUpdateRequest req)
        {
            var userIdRaw = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdRaw, out var userId) || userId == Guid.Empty)
                throw new ValidationAppException("UserId este obligatoriu.");

            var isAdmin = User.IsInRole(SystemRoles.Admin);

            var updated = await _command.UpdateNote(id, req, userId, isAdmin);
            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Policy = "write:note")]
        [SwaggerOperation(
            Summary = "Delete note",
            Description = "Deletes a note owned by the current user. Admin can delete any note.",
            OperationId = "Notes_Delete")]
        [SwaggerResponse(StatusCodes.Status200OK, "Note deleted")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Authentication required")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Note not found")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var userIdRaw = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdRaw, out var userId) || userId == Guid.Empty)
                throw new ValidationAppException("UserId este obligatoriu.");

            var isAdmin = User.IsInRole(SystemRoles.Admin);

            await _command.DeleteNote(id, userId, isAdmin);
            return Ok(new { message = "Deleted" });
        }
    }
}
