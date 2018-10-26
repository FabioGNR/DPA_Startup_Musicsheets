using DPA_Musicsheets.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models.Domain
{
    public class TimeSignature : Token
    {
        public int Count { get; set; }
        public Denominator Denominator { get; set; }

        public override void Accept(ITokenVisitor visitor)
        {
            visitor.ProcessToken(this);
        }

        public override bool Equals(object obj)
        {
            var signature = obj as TimeSignature;
            return signature != null &&
                   Count == signature.Count &&
                   Denominator.Equals(signature.Denominator);
        }

        public override int GetHashCode()
        {
            var hashCode = 1801049655;
            hashCode = hashCode * -1521134295 + Count.GetHashCode();
            hashCode = hashCode * -1521134295 + Denominator.GetHashCode();
            return hashCode;
        }
    }
}
