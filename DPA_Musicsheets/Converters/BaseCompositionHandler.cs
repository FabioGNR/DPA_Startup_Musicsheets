using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Models.Domain;

namespace DPA_Musicsheets.Converters
{
    public abstract class BaseCompositionHandler : ICompositionHandlerChain
    {
        public ICompositionHandlerChain Next { get; set; }

        public void Handle(Composition composition)
        {
            TryHandle(composition);
            Next?.Handle(composition);
        }

        protected abstract void TryHandle(Composition composition);
    }
}
