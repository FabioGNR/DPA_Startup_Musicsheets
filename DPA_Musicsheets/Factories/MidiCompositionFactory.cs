using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Models.Domain;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.Factories
{
    class MidiCompositionFactory : ICompositionFactory
    {
        public Composition ReadComposition(string fileName)
        {
            var midiSequence = new Sequence();
            midiSequence.Load(fileName);

            MidiComposer midiComposer = new MidiComposer(midiSequence);

            return midiComposer.Compose();
        }
    }
}
