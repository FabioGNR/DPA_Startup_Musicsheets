using DPA_Musicsheets.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Converters
{
    public interface ITokenVisitor
    {
        void ProcessToken(Note note);
        void ProcessToken(Rest rest);
        void ProcessToken(Barline barLine);
        void ProcessToken(TimeSignature timeSignature);
        void ProcessToken(Tempo tempo);
        void ProcessToken(Token any);
    }
}
