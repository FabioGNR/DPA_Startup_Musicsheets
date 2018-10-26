using DPA_Musicsheets.Editing.Memento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models.Domain
{
    public class Composition
    {
        public List<Token> Tokens { get; set; }
        public string FileName { get; set; }
        public string Title { get; set; }

        public Composition()
        {
            Tokens = new List<Token>();
        }

        public void Restore(CompositionMemento mem)
        {
            Tokens = mem.Tokens;
            FileName = mem.FileName;
            Title = mem.Title;
        }

        public override bool Equals(object other)
        {
            if (other == null) return false;
            if (other is CompositionMemento compMemento)
            {
                return Tokens.SequenceEqual(compMemento.Tokens) &&
                       FileName == compMemento.FileName &&
                       Title == compMemento.Title;
            }
            else if (other is Composition comp)
            {
                return Tokens.SequenceEqual(comp.Tokens) &&
                       FileName == comp.FileName &&
                       Title == comp.Title;
            }
            else return false;
        }

        public override int GetHashCode()
        {
            var hashCode = -1719507308;
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Token>>.Default.GetHashCode(Tokens);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FileName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Title);
            return hashCode;
        }
    }
}
