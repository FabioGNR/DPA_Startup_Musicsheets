using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models.Domain
{
    public class Denominator
    {
        public int Value { get; private set; }

        public Denominator(int denominator)
        {
            if (Math.Log(denominator, 2) % 1 != 0)
            {
                throw new ArgumentException("Value must be equal to a power of 2", nameof(denominator));
            }

            Value = denominator;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override bool Equals(object obj)
        {
            var denominator = obj as Denominator;
            return denominator != null &&
                   Value == denominator.Value;
        }

        public override int GetHashCode()
        {
            return -1937169414 + Value.GetHashCode();
        }
    }
}
