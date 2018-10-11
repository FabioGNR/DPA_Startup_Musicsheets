using DPA_Musicsheets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Factories
{
    public static class AbstractCompositionFactory
    {
        private static readonly Dictionary<string, ICompositionFactory> fileFactories =
            new Dictionary<string, ICompositionFactory>();

        static AbstractCompositionFactory()
        {
            fileFactories.Add(".mid", new MidiCompositionFactory());
            fileFactories.Add(".ly", new LilypondFileCompositionFactory());
        }

        public static ICompositionFactory GetFactory(string extension)
        {
            if (fileFactories.ContainsKey(extension))
            {
                return fileFactories[extension];
            }
            else
            {
                throw new NotSupportedException($"No support for filetype {extension}");
            }
        }
    }
}
