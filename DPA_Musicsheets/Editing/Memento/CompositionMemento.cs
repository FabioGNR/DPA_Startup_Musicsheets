using DPA_Musicsheets.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Editing.Memento
{
    public class CompositionMemento
    {
        public List<Token> Tokens { get; set; }
        public string FileName { get; set; }
        public string Title { get; set; }

        public CompositionMemento(List<Token> tokens, string fileName, string title)
        {
            Tokens = tokens;
            FileName = fileName;
            title = Title;
        }

        public CompositionMemento(Composition comp)
        {
            Tokens = comp.Tokens;
            FileName = comp.FileName;
            Title = comp.Title;
        }
    }
}
