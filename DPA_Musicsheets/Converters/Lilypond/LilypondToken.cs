using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Converters.Lilypond
{
    class LilypondToken
    {
        public LilypondTokenKind Kind { get; set; }
        public string TokenText { get; set; }
    }
}
