using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models.Domain
{
    public class Pitch
    {
        public Tone Tone { get; set; }
        public int OctaveOffset { get; set; } = 0;
        public Accidental Accidental { get; set; }

        public override bool Equals(object obj)
        {
            var pitch = obj as Pitch;
            return pitch != null &&
                   Tone == pitch.Tone &&
                   OctaveOffset == pitch.OctaveOffset &&
                   Accidental == pitch.Accidental;
        }

        public override int GetHashCode()
        {
            var hashCode = -487476052;
            hashCode = hashCode * -1521134295 + Tone.GetHashCode();
            hashCode = hashCode * -1521134295 + OctaveOffset.GetHashCode();
            hashCode = hashCode * -1521134295 + Accidental.GetHashCode();
            return hashCode;
        }
    }
}
