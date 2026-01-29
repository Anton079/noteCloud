namespace NoteCloud_api.Notes.Dto
{
    public class NoteRequest
    {
        public string Title { get; set; } = default!;
        public string Content { get; set; } = default!;
        public string CategoryId { get; set; } = default!;
        public bool IsFavorite { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
