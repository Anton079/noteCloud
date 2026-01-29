using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteCloud_api.Categories.Dto;
using NoteCloud_api.Categories.Service;
using Swashbuckle.AspNetCore.Annotations;

namespace NoteCloud_api.Categories.Controllers
{
    [ApiController]
    [Route("api/v1/categories")]
    [SwaggerTag("Category CRUD for grouping notes")]
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
        [Authorize(Policy = "read:category")]
        [SwaggerOperation(
            Summary = "List categories",
            Description = "Returns all categories. Typically used to filter notes by category.",
            OperationId = "Categories_GetAll")]
        [SwaggerResponse(StatusCodes.Status200OK, "Categories list", typeof(CategoryListRequest))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Authentication required")]
        public async Task<ActionResult<CategoryListRequest>> GetAll()
        {
            var categories = await _query.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Policy = "read:category")]
        [SwaggerOperation(
            Summary = "Get category by id",
            Description = "Returns a single category by id.",
            OperationId = "Categories_GetById")]
        [SwaggerResponse(StatusCodes.Status200OK, "Category found", typeof(CategoryResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Authentication required")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Category not found")]
        public async Task<ActionResult<CategoryResponse>> GetById(Guid id)
        {
            var category = await _query.FindCategoryByIdAsync(id);
            return Ok(category);
        }

        [HttpPost]
        [Authorize(Policy = "write:category")]
        [SwaggerOperation(
            Summary = "Create category",
            Description = "Creates a new category for grouping notes.",
            OperationId = "Categories_Create")]
        [SwaggerResponse(StatusCodes.Status201Created, "Category created", typeof(CategoryResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Validation failed")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Authentication required")]
        public async Task<ActionResult<CategoryResponse>> Create([FromBody] CategoryRequest req)
        {
            var created = await _command.CreateCategory(req);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Policy = "write:category")]
        [SwaggerOperation(
            Summary = "Update category",
            Description = "Updates a category by id.",
            OperationId = "Categories_Update")]
        [SwaggerResponse(StatusCodes.Status200OK, "Category updated", typeof(CategoryResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Validation failed")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Authentication required")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Category not found")]
        public async Task<ActionResult<CategoryResponse>> Update(Guid id, [FromBody] CategoryUpdateRequest req)
        {
            var updated = await _command.UpdateCategory(id, req);
            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Policy = "write:category")]
        [SwaggerOperation(
            Summary = "Delete category",
            Description = "Deletes a category by id. Notes using this category must be reassigned first.",
            OperationId = "Categories_Delete")]
        [SwaggerResponse(StatusCodes.Status200OK, "Category deleted")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Authentication required")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Category not found")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _command.DeleteCategory(id);
            return Ok(new { message = "Deleted" });
        }
    }
}
