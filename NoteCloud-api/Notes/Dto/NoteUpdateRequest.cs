namespace NoteCloud_api.Notes.Dto
{
    public class NoteUpdateRequest
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Category { get; set; }
        public bool? IsFavorite { get; set; }
        public DateTime? Date { get; set; }
    }
}
