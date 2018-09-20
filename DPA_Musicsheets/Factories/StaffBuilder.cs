using DPA_Musicsheets.Models.Domain;
using PSAMControlLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Factories
{
    class StaffBuilder
    {
        private readonly Composition composition;

        public StaffBuilder(Composition composition)
        {
            this.composition = composition;
        }

        //public IEnumerable<MusicalSymbol> Convert()
        //{
        //    foreach (Token token in composition.Tokens)
        //    {

        //    }
        //}
    }
}
