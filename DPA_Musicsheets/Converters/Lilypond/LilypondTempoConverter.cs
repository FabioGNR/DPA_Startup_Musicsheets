using DPA_Musicsheets.Builders;
using DPA_Musicsheets.Models.Domain;

namespace DPA_Musicsheets.Converters.Lilypond
{
    internal class LilypondTempoConverter : ILilypondTokenConverter
    {
        public Token Convert(LilypondTokenEnumerator enumerator)
        {
            enumerator.Next();
            var builder = new TempoBuilder();
            //TODO: read bpm from the next token
            builder.WithBPM(120);
            return builder.Build();
        }
    }
}