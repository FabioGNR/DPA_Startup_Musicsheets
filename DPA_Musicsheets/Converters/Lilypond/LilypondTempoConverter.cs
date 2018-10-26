using DPA_Musicsheets.Builders;
using DPA_Musicsheets.Models.Domain;
using System;
using System.Text.RegularExpressions;

namespace DPA_Musicsheets.Converters.Lilypond
{
    internal class LilypondTempoConverter : ILilypondTokenConverter
    {
        public Token Convert(LilypondTokenEnumerator enumerator)
        {
            enumerator.Next();
            var builder = new TempoBuilder();
            int bpm = 120;
            if (enumerator.Current != null)
            {
                var match = Regex.Match(enumerator.Current.TokenText, @"\d+=(\d+)");
                if (match.Success)
                {
                    int.TryParse(match.Groups[1].Value, out bpm);
                }
            }
            bpm = Math.Max(1, bpm); // make sure no <= 0 bpm is set
            builder.WithBPM(bpm);
            return builder.Build();
        }
    }
}