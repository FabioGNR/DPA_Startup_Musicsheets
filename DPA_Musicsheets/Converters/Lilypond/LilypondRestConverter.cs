using DPA_Musicsheets.Builders;
using DPA_Musicsheets.Models.Domain;
using System;
using System.Text.RegularExpressions;

namespace DPA_Musicsheets.Converters.Lilypond
{
    internal class LilypondRestConverter : ILilypondTokenConverter
    {
        private static Regex regex = new Regex(@"r(\d)(\.*)");

        public Token Convert(LilypondTokenEnumerator enumerator)
        {
            var input = enumerator.Current;
            var match = regex.Match(input.TokenText);
            if (match.Success)
            {
                SymbolBuilder builder = new SymbolBuilder();
                if (int.TryParse(match.Groups[1].Value, out int denominator))
                {
                    int dots = match.Groups[2].Length;
                    builder.WithLength(denominator, dots);
                    return builder.Build();
                }
                else
                {
                    throw new InvalidOperationException("Not a rest that can be converted");
                }
            }
            else
            {
                throw new InvalidOperationException("Not a rest that can be converted");
            }
        }
    }
}