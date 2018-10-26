using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Converters;

namespace DPA_Musicsheets.Models.Domain
{
    public class Clef : Token
    {
        public ClefTone Tone { get; set; }
        public int Bar { get; set; }
        public override void Accept(ITokenVisitor visitor)
        {
            visitor.ProcessToken(this);
        }

        public override bool Equals(object obj)
        {
            var clef = obj as Clef;
            return clef != null &&
                   Tone == clef.Tone &&
                   Bar == clef.Bar;
        }

        public override int GetHashCode()
        {
            var hashCode = -153730685;
            hashCode = hashCode * -1521134295 + Tone.GetHashCode();
            hashCode = hashCode * -1521134295 + Bar.GetHashCode();
            return hashCode;
        }
    }
}
