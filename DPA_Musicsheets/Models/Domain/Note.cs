﻿using DPA_Musicsheets.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models.Domain
{
    public class Note : Symbol
    {
        public Pitch Pitch { get; set; }
        public override void Accept(ITokenVisitor visitor)
        {
            visitor.ProcessToken(this);
        }
    }
}
