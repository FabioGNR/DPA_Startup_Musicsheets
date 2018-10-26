using DPA_Musicsheets.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Managers
{
    public class CompositionChangedArgs
    {
        /// <summary>
        /// The newly built composition
        /// </summary>
        public Composition NewComposition { get; set; }
        /// <summary>
        /// This bool indicates if the composition was loaded in and is 'clean'
        /// True if clean/fresh, false if not or unsure
        /// </summary>
        public bool IsFresh { get; set; }
    }
}
