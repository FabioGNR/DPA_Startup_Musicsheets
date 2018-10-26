using DPA_Musicsheets.Builders;
using DPA_Musicsheets.Models.Domain;
using System;

namespace DPA_Musicsheets.Converters.Lilypond
{
    internal class LilypondClefConverter : ILilypondTokenConverter
    {
        public Token Convert(LilypondTokenEnumerator enumerator)
        {
            enumerator.Next();
            ClefBuilder builder = new ClefBuilder();
            ClefTone tone = ClefTone.G;
            if(enumerator.Current != null)
            {
                Enum.TryParse(enumerator.Current.TokenText, true, out tone);
            }
            builder.WithTone(tone);
            return builder.Build();
        }
    }
}