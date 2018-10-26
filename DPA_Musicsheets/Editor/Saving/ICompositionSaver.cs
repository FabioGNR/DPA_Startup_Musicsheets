using DPA_Musicsheets.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Editor.Saving
{
    interface ICompositionSaver
    {
        string Description { get; }
        void Save(Composition composition, string filename);
    }
}
