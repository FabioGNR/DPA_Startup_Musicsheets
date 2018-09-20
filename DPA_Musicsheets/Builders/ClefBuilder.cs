using DPA_Musicsheets.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Builders
{
    public class ClefBuilder
    {
        private ClefTone tone = ClefTone.Unknown;
        private int bar = 0;
        public ClefBuilder WithTone(ClefTone tone)
        {
            this.tone = tone;
            return this;
        }

        public ClefBuilder OnBar(int bar)
        {
            if (bar < 1 || bar > 5)
            {
                throw new ArgumentException("Bar must be between 1 and 5");
            }
            this.bar = bar;
            return this;
        }

        public Clef Build()
        {
            if (tone == ClefTone.Unknown)
            {
                throw new InvalidOperationException("No clef tone set");
            }
            if (bar == 0)
            {
                throw new InvalidOperationException("No bar set");
            }
            return new Clef { Tone = tone, Bar = bar };
        }
    }
}
