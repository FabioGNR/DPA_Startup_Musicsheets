using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Converters;
using DPA_Musicsheets.Models.Domain;

namespace DPA_Musicsheets.Editor.Saving
{
    class LilypondSaver : ICompositionSaver
    {
        public string Description => "Lilypond";

        public void Save(Composition composition, string filename)
        {
            var visitor = new ToLilypondVisitor();
            foreach (var token in composition.Tokens)
            {
                token.Accept(visitor);
            }
            string lilypondText = visitor.Build();
            File.WriteAllText(filename, lilypondText);
        }
    }
}
