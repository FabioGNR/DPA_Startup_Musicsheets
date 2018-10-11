using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Converters;
using DPA_Musicsheets.Models.Domain;

namespace DPA_Musicsheets.Factories
{
    abstract class BaseCompositionFactory : ICompositionFactory
    {
        ICompositionHandlerChain chainHandler = new InsertBarCompositionHandler();
        public Composition ReadComposition(string fileName)
        {
            Composition composition = CreateComposition(fileName);
            chainHandler?.Handle(composition);
            return composition;
        }

        protected abstract Composition CreateComposition(string fileName);
    }
}
