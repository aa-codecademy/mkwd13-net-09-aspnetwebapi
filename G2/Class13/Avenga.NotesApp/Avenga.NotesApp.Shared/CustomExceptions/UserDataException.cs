namespace Avenga.NotesApp.Shared.CustomExceptions
{
    public class UserDataException : Exception
    {
        public UserDataException(string message) : base(message) { }
    }
}
