﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models.Domain
{
    class TimeSignature : Token
    {
        public int Count { get; set; }
        public Denominator Denominator { get; set; }
    }
}