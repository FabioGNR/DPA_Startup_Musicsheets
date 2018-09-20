using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Converters;

namespace DPA_Musicsheets.Models.Domain
{
    public class Barline : Token
    {
        public override void Accept(ITokenVisitor visitor)
        {
            visitor.ProcessToken(this);
        }
    }
}
