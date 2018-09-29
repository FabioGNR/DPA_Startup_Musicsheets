using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Converters.Lilypond
{
    class LilypondTokenEnumerator
    {
        private readonly IEnumerator<LilypondToken> _tokens;
        public bool HasTokensLeft { get; private set; }
        public LilypondToken Current => _tokens.Current;
        public LilypondTokenEnumerator(IEnumerable<LilypondToken> tokens)
        {
            _tokens = tokens.GetEnumerator();
            _tokens.MoveNext();
            HasTokensLeft = _tokens.Current != null;
        }

        /// <summary>
        /// Moves to next token
        /// </summary>
        /// <returns>token before moving to next</returns>
        public LilypondToken Next()
        {
            if (HasTokensLeft) { 
                var token = _tokens.Current;
                HasTokensLeft = _tokens.MoveNext();
                return token;
            }
            return null;
        }
    }
}
