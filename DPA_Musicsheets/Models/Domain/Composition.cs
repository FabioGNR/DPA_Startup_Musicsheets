﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models.Domain
{
    public class Composition
    {
        public List<Token> Tokens { get; set; }
        public string FileName { get; set; }
        public string Title { get; set; }
    }
}