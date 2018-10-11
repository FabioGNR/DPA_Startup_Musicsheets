using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Converters.Lilypond;
using DPA_Musicsheets.Models.Domain;

namespace DPA_Musicsheets.Factories
{
    class LilypondCompositionFactory : BaseCompositionFactory
    {
        protected override Composition CreateComposition(string lilypondText)
        {
            var tokenizer = new LilypondTokenizer();
            var lilypondTokens = tokenizer.Read(lilypondText);
            var composer = new LilypondComposer();
            var composition = composer.Compose(lilypondTokens);
            return composition;
        }
    }
}
