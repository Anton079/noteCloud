using Swashbuckle.AspNetCore.Annotations;

namespace NoteCloud_api.Categories.Dto
{
    public class CategoryRequest
    {
     
        public string Name { get; set; } = default!;
    }
}
