namespace Avenga.NotesApp.Shared.CustomExceptions
{
    public class NoteNotFoundException : Exception
    {
        public NoteNotFoundException(string message) :base(message) { }
    }
}
