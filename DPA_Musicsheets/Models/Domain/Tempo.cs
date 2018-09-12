using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models.Domain
{
    class Tempo
    {
        public uint BPM { get; set; }

        public Tempo(uint bpm)
        {
            if (bpm == 0)
            {
                throw new ArgumentException("BPM must be over 0", nameof(bpm));
            }
        }
    }
}
