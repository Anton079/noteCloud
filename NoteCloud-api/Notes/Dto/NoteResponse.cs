namespace NoteCloud_api.Notes.Dto
{
    public class NoteResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public string Content { get; set; } = default!;
        public Guid CategoryId { get; set; }
        public bool IsFavorite { get; set; }
        public DateTime Date { get; set; }
        public Guid UserId { get; set; }
    }
}
