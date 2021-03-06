﻿using DPA_Musicsheets.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models.Domain
{
    public class Tempo : Token
    {
        public uint BPM { get; set; }

        public Tempo(int bpm)
        {
            if (bpm <= 0)
            {
                throw new ArgumentException("BPM must be over 0", nameof(bpm));
            }
            BPM = Convert.ToUInt32(bpm);
        }

        public override void Accept(ITokenVisitor visitor)
        {
            visitor.ProcessToken(this);
        }

        public override bool Equals(object obj)
        {
            var tempo = obj as Tempo;
            return tempo != null &&
                   BPM == tempo.BPM;
        }

        public override int GetHashCode()
        {
            return 1651896086 + BPM.GetHashCode();
        }
    }
}
