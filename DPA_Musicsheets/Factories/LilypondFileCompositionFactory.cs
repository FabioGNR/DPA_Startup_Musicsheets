using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Models.Domain;

namespace DPA_Musicsheets.Factories
{
    class LilypondFileCompositionFactory : ICompositionFactory
    {
        public Composition ReadComposition(string fileName)
        {
            string lilypondText = File.ReadAllText(fileName);
            return new LilypondCompositionFactory().ReadComposition(lilypondText);
        }
    }
}
