using DPA_Musicsheets.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models.Domain
{
    public class Note : Symbol
    {
        public Pitch Pitch { get; set; }
        public override void Accept(ITokenVisitor visitor)
        {
            visitor.ProcessToken(this);
        }

        public override bool Equals(object obj)
        {
            var note = obj as Note;
            return note != null &&
                   base.Equals(obj) &&
                   Pitch.Equals(note.Pitch);
        }

        public override int GetHashCode()
        {
            var hashCode = -1651747865;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Pitch.GetHashCode();
            return hashCode;
        }
    }
}
