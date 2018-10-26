using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models.Domain
{
    /// <summary>
    /// Contains possible ClefTones
    /// <remarks>Contains duplicates, e.g G and Treble</remarks>
    /// </summary>
    public enum ClefTone
    {
        Unknown = 0,
        G = 2,
        F = 4,
        C = 3,
        Treble = 2,
        Bass = 4,
        Alto = 3,
    }
}
