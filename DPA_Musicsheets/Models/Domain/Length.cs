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

        public override bool Equals(object obj)
        {
            var length = obj as Length;
            return length != null &&
                   Denominator.Equals(length.Denominator) &&
                   AmountOfDots == length.AmountOfDots;
        }

        public override int GetHashCode()
        {
            var hashCode = -780910197;
            hashCode = hashCode * -1521134295 + Denominator.GetHashCode();
            hashCode = hashCode * -1521134295 + AmountOfDots.GetHashCode();
            return hashCode;
        }
    }
}
