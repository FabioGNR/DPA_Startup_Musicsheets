﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models.Domain
{
    public abstract class Symbol : Token
    {
        public Length Length { get; set; }
    }
}
