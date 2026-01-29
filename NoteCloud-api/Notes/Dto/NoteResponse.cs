namespace NoteCloud_api.Notes.Dto
{
    public class NoteResponse
    {
        public string Id { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string Content { get; set; } = default!;
        public string CategoryId { get; set; } = default!;
        public bool IsFavorite { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; } = default!;
    }
}
