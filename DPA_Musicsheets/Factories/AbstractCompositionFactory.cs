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
        public static ICompositionFactory GetFactory(FileType type)
        {
            switch (type)
            {
                case FileType.LilyPond:
                    return new LilyPondCompositionFactory();
                case FileType.Midi:
                    return new MidiCompositionFactory();
                default:
                    throw new NotImplementedException($"{type}-support is not yet implemented");

            }

        }
    }
}
