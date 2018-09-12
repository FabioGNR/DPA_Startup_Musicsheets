using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models.Domain
{
    class Length
    {
        public int Denominator { get; set; } // Length of the Symbol is 1/Denominator
        public bool Dotted { get; set; } // Is Note one and a half times as long
    }
}
