using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteCloud_api.Categories.Dto;
using NoteCloud_api.Categories.Service;

namespace NoteCloud_api.Categories.Controllers
{
    [ApiController]
    [Route("api/v1/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICommandServiceCategory _command;
        private readonly IQueryServiceCategory _query;

        public CategoriesController(ICommandServiceCategory command, IQueryServiceCategory query)
        {
            _command = command;
            _query = query;
        }

        [HttpGet]
        [Authorize(Policy = "read:note")]
        public async Task<ActionResult<CategoryListRequest>> GetAll()
        {
            try
            {
                var categories = await _query.GetAllCategoriesAsync();
                return Ok(categories);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Internal server error." });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "read:note")]
        public async Task<ActionResult<CategoryResponse>> GetById(string id)
        {
            try
            {
                var category = await _query.FindCategoryByIdAsync(id);
                return Ok(category);
            }
            catch (ArgumentException ex)
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
        public async Task<ActionResult<CategoryResponse>> Create([FromBody] CategoryRequest req)
        {
            try
            {
                var created = await _command.CreateCategory(req);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
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
        public async Task<ActionResult<CategoryResponse>> Update(string id, [FromBody] CategoryUpdateRequest req)
        {
            try
            {
                var updated = await _command.UpdateCategory(id, req);
                return Ok(updated);
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
                await _command.DeleteCategory(id);
                return Ok(new { message = "Deleted" });
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
    }
}
