using DPA_Musicsheets.Builders;
using DPA_Musicsheets.Models.Domain;

namespace DPA_Musicsheets.Converters.Lilypond
{
    internal class LilypondTimeSignatureConverter : ILilypondTokenConverter
    {
        public Token Convert(LilypondTokenEnumerator enumerator)
        {
            enumerator.Next();
            var signature = enumerator.Current;
            string[] parts = signature.TokenText.Split('/');
            var builder = new TimeSignatureBuilder();
            if (int.TryParse(parts[0], out int count) && int.TryParse(parts[1], out int denominator))
            {
                builder.WithCount(count);
                builder.WithDenominator(denominator);
            }
            return builder.Build();
        }
    }
}