using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Converters.Lilypond
{
    enum LilypondTokenKind
    {
        Unknown,
        Relative,
        Clef,
        TimeSignature,
        Tempo,
        Note,
        Rest,
        Repeat,
        Alternative,
        Barline,
    }
}
