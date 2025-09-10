using Avenga.NotesApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avenga.NotesApp.Dto
{
    public class NoteDto
    {
        public int Id {  get; set; }
        public string Text { get; set; }
        public Priority Priority { get; set; }
        public Tag Tag { get; set; }
        public string UserFullName { get; set; }
    }
}
