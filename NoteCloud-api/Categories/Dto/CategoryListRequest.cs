namespace NoteCloud_api.Categories.Dto
{
    public class CategoryListRequest
    {
        public List<CategoryResponse> Categories { get; set; } = new();
    }
}
