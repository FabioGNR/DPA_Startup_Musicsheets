using DPA_Musicsheets.Builders;
using DPA_Musicsheets.Models.Domain;

namespace DPA_Musicsheets.Converters.Lilypond
{
    internal class LilypondClefConverter : ILilypondTokenConverter
    {
        public Token Convert(LilypondToken input)
        {
            ClefBuilder builder = new ClefBuilder();
            builder.OnBar(4);
            builder.WithTone(ClefTone.G);
            return builder.Build();
        }
    }
}