using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<List<NoteResponse>>> GetAll([FromQuery] string? category)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(category))
                {
                    var byCategory = await _query.GetNotesByCategoryAsync(category);
                    return Ok(byCategory);
                }

                var notes = await _query.GetAllNotesAsync();
                return Ok(notes);
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

        [HttpGet("{id:int}")]
        [Authorize(Policy = "read:note")]
        public async Task<ActionResult<NoteResponse>> GetById(int id)
        {
            try
            {
                var note = await _query.FindNoteByIdAsync(id);
                return Ok(note);
            }
            catch (NoteNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Policy = "write:note")]
        public async Task<ActionResult<NoteResponse>> Create([FromBody] NoteRequest req)
        {
            try
            {
                var created = await _command.CreateNote(req);
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
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpPut("{id:int}")]
        [Authorize(Policy = "write:note")]
        public async Task<ActionResult<NoteResponse>> Update(int id, [FromBody] NoteUpdateRequest req)
        {
            try
            {
                var updated = await _command.UpdateNote(id, req);
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
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "write:note")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _command.DeleteNote(id);
                return Ok(new { message = "Deleted" });
            }
            catch (NoteNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
