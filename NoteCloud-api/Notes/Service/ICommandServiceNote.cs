using NoteCloud_api.Notes.Dto;

namespace NoteCloud_api.Notes.Service
{
    public interface ICommandServiceNote
    {
        Task<NoteResponse> CreateNote(NoteRequest req);
        Task<NoteResponse> UpdateNote(int id, NoteUpdateRequest req);
        Task<bool> DeleteNote(int id);
    }
}
