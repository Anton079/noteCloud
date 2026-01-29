using Swashbuckle.AspNetCore.Annotations;

namespace NoteCloud_api.Notes.Dto
{
    public class NoteCreateRequest
    {
        public string Title { get; set; } = default!;
        
        public string Content { get; set; } = default!;

       
        public Guid CategoryId { get; set; }
        
        public bool IsFavorite { get; set; }
    }
}
