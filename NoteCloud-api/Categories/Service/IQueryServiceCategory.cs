using NoteCloud_api.Categories.Dto;

namespace NoteCloud_api.Categories.Service
{
    public interface IQueryServiceCategory
    {
        Task<CategoryResponse> FindCategoryByIdAsync(string id);
        Task<CategoryListRequest> GetAllCategoriesAsync();
    }
}
