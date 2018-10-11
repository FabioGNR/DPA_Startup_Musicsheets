using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Converters;
using DPA_Musicsheets.Models.Domain;

namespace DPA_Musicsheets.Editor.Saving
{
    class MidiSaver : ICompositionSaver
    {
        public string Description => "MIDI";

        public void Save(Composition composition, string filename)
        {
            var visitor = new ToMidiVisitor();
            foreach (var token in composition.Tokens)
            {
                token.Accept(visitor);
            }
            var sequence = visitor.Sequence;
            sequence.Save(filename);
        }
    }
}
