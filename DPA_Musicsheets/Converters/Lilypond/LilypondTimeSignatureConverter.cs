using DPA_Musicsheets.Builders;
using DPA_Musicsheets.Models.Domain;

namespace DPA_Musicsheets.Converters.Lilypond
{
    internal class LilypondTimeSignatureConverter : ILilypondTokenConverter
    {
        public Token Convert(LilypondToken input)
        {
            var builder = new TimeSignatureBuilder();
            builder.WithCount(4);
            builder.WithDenominator(4);
            return builder.Build();
        }
    }
}