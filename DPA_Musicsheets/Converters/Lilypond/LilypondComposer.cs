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
                {LilypondTokenKind.TimeSignature, new LilypondTimeSignatureConverter() },
                {LilypondTokenKind.Relative, new LilypondRelativeConverter() }
            };

        public Composition Compose(IEnumerable<LilypondToken> lilypondTokens)
        {
            composition = new Composition();
            var enumerator = new LilypondTokenEnumerator(lilypondTokens);
            while(enumerator.HasTokensLeft)
            {
                AddToComposition(enumerator);
            }
            return composition;
        }

        private void AddToComposition(LilypondTokenEnumerator enumerator)
        {
            var input = enumerator.Current;
            if (_tokenConverters.ContainsKey(input.Kind))
            {
                var converter = _tokenConverters[input.Kind];
                var token = converter.Convert(enumerator);
                if (token != null)
                {
                    composition.Tokens.Add(token);
                }
            }
            enumerator.Next();
        }
    }
}
