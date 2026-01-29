namespace NoteCloud_api.Notes.Dto
{
    public class NoteListRequest
    {
        public List<NoteResponse> Notes { get; set; } = new();
    }
}
