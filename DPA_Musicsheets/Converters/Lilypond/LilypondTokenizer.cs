using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Converters.Lilypond
{
    class LilypondTokenizer
    {
        private readonly Dictionary<Regex, LilypondTokenKind>
            _tokenLookup = new Dictionary<Regex, LilypondTokenKind>
            {
                {new Regex(@"(~?)([a-g])(is|es)?([,']*)(\d+)(\.*)"), LilypondTokenKind.Note },
                {new Regex(@"r\d"), LilypondTokenKind.Rest },
                {new Regex(@"\\clef"), LilypondTokenKind.Clef },
                {new Regex(@"\\time"), LilypondTokenKind.TimeSignature },
                {new Regex(@"\\tempo"), LilypondTokenKind.Tempo },
                {new Regex(@"\\repeat"), LilypondTokenKind.Repeat },
                {new Regex(@"\\alternative"), LilypondTokenKind.Alternative },
                {new Regex(@"\|"), LilypondTokenKind.Barline },
                {new Regex(@"\\relative"), LilypondTokenKind.Relative },
                {new Regex(@".*"), LilypondTokenKind.Unknown }
            };

        // Read string, split on space and nl => tokens
        public IEnumerable<LilypondToken> Read(string lilyString)
        {
            var strTokens = SplitInput(lilyString);
            return strTokens.Select(st => CreateToken(st));
        }

        private LilypondToken CreateToken(string tokenizedString)
        {
            var matchedTokenKind = _tokenLookup.First(l => l.Key.IsMatch(tokenizedString));
            return new LilypondToken
            {
                Kind = matchedTokenKind.Value,
                TokenText = tokenizedString
            };
        }

        private IEnumerable<string> SplitInput(string input)
        {
            return input.Split(' ', '\n', '\r');
        }
    }
}
