using DPA_Musicsheets.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Builders
{
    public class SymbolBuilder
    {
        private Length length;
        private Pitch pitch;

        public void WithLength(int denominator, int amountOfDots)
        {
            length = new Length
            {
                Denominator = new Denominator(denominator),
                AmountOfDots = amountOfDots
            };
        }

        public void WithPitch(Tone tone, Accidental acc, int OctaveOffset)
        {
            pitch = new Pitch
            {
                Tone = tone,
                Accidental = acc,
                OctaveOffset = OctaveOffset
            };
        }

        public Symbol Build()
        {
            if (length != null && pitch != null)
            {
                return new Note
                {
                    Length = length,
                    Pitch = pitch
                };
            }
            else if (length != null)
            {
                return new Rest
                {
                    Length = length
                };
            }
            else
            {
                throw new InvalidOperationException("Symbol is not complete, please add a length");
            }
        }
    }
}
