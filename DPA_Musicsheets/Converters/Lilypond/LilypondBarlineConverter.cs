using DPA_Musicsheets.Models.Domain;

namespace DPA_Musicsheets.Converters.Lilypond
{
    internal class LilypondBarlineConverter : ILilypondTokenConverter
    {
        public Token Convert(LilypondTokenEnumerator enumerator)
        {
            return new Barline();
        }
    }
}