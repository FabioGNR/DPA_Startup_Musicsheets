using DPA_Musicsheets.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Builders
{
    public class TempoBuilder
    {
        private int BPM = 0;

        public TempoBuilder WithBPM(int bpm)
        {
            if (bpm <= 0)
            {
                throw new ArgumentException("Provide a bpm larger than 0");
            }
            BPM = bpm;
            return this;
        }

        public Tempo Build()
        {
            return new Tempo(BPM);
        }
    }
}
