using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models.Domain
{
    class Pitch
    {
        public Tone Tone { private get; set; }
        public int OctaveOffset { get; set; } = 0;
    }
}
