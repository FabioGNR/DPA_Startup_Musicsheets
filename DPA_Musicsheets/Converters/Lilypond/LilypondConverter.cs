using DPA_Musicsheets.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Converters.Lilypond
{
    class LilypondComposer
    {
        private readonly Dictionary<LilypondTokenKind, ILilypondTokenConverter> _tokenConverters =
            new Dictionary<LilypondTokenKind, ILilypondTokenConverter>
            {
                {LilypondTokenKind.Barline, new LilypondBarlineConverter() },
                {LilypondTokenKind.Alternative, new LilypondAlternativeConverter() },
                {LilypondTokenKind.Clef, new LilypondClefConverter() },
                {LilypondTokenKind.Note, new LilypondNoteConverter() },
                {LilypondTokenKind.Relative, new LilypondRelativeConverter() },
                {LilypondTokenKind.Repeat, new LilypondRepeatConverter() },
                {LilypondTokenKind.Rest, new LilypondRestConverter() },
                {LilypondTokenKind.Tempo, new LilypondTempoConverter() },
                {LilypondTokenKind.TimeSignature, new LilypondTimeSignatureConverter() }
            };

        public Composition Compose()
        {

        }
    }
}
