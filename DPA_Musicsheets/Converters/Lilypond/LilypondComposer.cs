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
        private Composition composition;
        private readonly Dictionary<LilypondTokenKind, ILilypondTokenConverter> _tokenConverters =
            new Dictionary<LilypondTokenKind, ILilypondTokenConverter>
            {
                {LilypondTokenKind.Barline, new LilypondBarlineConverter() },
                {LilypondTokenKind.Clef, new LilypondClefConverter() },
                {LilypondTokenKind.Note, new LilypondNoteConverter() },
                {LilypondTokenKind.Rest, new LilypondRestConverter() },
                {LilypondTokenKind.Tempo, new LilypondTempoConverter() },
                {LilypondTokenKind.TimeSignature, new LilypondTimeSignatureConverter() }
            };

        public Composition Compose(IEnumerable<LilypondToken> lilypondTokens)
        {
            composition = new Composition();
            foreach (var lilypondToken in lilypondTokens)
            {
                AddToComposition(lilypondToken);
            }
            return composition;
        }

        private void AddToComposition(LilypondToken input)
        {
            if (_tokenConverters.ContainsKey(input.Kind))
            {
                var converter = _tokenConverters[input.Kind];
                var token = converter.Convert(input);
                composition.Tokens.Add(token);
            }
        }
    }
}
