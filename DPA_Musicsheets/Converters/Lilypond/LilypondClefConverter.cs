using DPA_Musicsheets.Builders;
using DPA_Musicsheets.Models.Domain;

namespace DPA_Musicsheets.Converters.Lilypond
{
    internal class LilypondClefConverter : ILilypondTokenConverter
    {
        public Token Convert(LilypondTokenEnumerator enumerator)
        {
            enumerator.Next();
            ClefBuilder builder = new ClefBuilder();
            builder.OnBar(2);
            builder.WithTone(ClefTone.G);
            return builder.Build();
        }
    }
}