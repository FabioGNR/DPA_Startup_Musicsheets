using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models.Domain
{
    class Note : Symbol
    {
        Pitch Pitch { get; set; }
    }
}
