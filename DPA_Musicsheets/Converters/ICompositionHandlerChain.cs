using DPA_Musicsheets.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Converters
{
    public interface ICompositionHandlerChain
    {
        ICompositionHandlerChain Next { get; set; }

        void Handle(Composition composition);
    }
}
