using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avenga.NotesApp.Shared
{
    public class NoteDataException : Exception
    {
        public NoteDataException(string message) : base(message) { }
    }
}
