using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models.Domain
{
    public class Length
    {
        public Denominator Denominator { get; set; } // Length of the Symbol is 1/Denominator
        public int AmountOfDots { get; set; } // How many Dots does this symbol have?
    }
}
