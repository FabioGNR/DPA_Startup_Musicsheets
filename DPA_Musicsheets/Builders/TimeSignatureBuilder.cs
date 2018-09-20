using DPA_Musicsheets.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Builders
{
    class TimeSignatureBuilder
    {
        private int Count = 0;
        private Denominator Denominator;

        public TimeSignatureBuilder WithCount(int Count)
        {
            this.Count = Count;
            return this;
        }

        public TimeSignatureBuilder WithDenominator(int Denominator)
        {
            this.Denominator = new Denominator(Denominator);
            return this;
        }

        public TimeSignature Build()
        {
            if (Count <= 0)
                throw new ArgumentException("Provide a Count higher than 0");
            if (Denominator == null)
                throw new InvalidOperationException("No denominator was given");
            else
            {
                return new TimeSignature
                {
                    Count = this.Count,
                    Denominator = this.Denominator
                };
            }
        }
    }
}
