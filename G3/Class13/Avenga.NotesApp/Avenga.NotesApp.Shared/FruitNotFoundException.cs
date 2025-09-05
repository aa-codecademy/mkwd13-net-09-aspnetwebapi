namespace Avenga.NotesApp.Shared
{
    public class FruitNotFoundException : Exception
    {
        public FruitNotFoundException(string message) : base(message) { }
    }
}
