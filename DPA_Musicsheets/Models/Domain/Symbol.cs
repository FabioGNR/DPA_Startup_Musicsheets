using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models.Domain
{
    public abstract class Symbol : Token
    {
        public Length Length { get; set; }

        public override bool Equals(object obj)
        {
            var symbol = obj as Symbol;
            return symbol != null &&
                   EqualityComparer<Length>.Default.Equals(Length, symbol.Length);
        }

        public override int GetHashCode()
        {
            return -2130075011 + EqualityComparer<Length>.Default.GetHashCode(Length);
        }
    }
}
