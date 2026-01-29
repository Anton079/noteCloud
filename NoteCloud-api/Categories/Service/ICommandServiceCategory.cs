using NoteCloud_api.Categories.Dto;

namespace NoteCloud_api.Categories.Service
{
    public interface ICommandServiceCategory
    {
        Task<CategoryResponse> CreateCategory(CategoryRequest req);
        Task<CategoryResponse> UpdateCategory(Guid id, CategoryUpdateRequest req);
        Task<bool> DeleteCategory(Guid id);
    }
}
