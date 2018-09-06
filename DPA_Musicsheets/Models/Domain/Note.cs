using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models.Domain
{
    class Note : Token
    {
        public Length Length { get; set; }
        public Pitch Pitch { get; set; }
    }
}
