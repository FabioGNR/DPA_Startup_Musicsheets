using DPA_Musicsheets.Converters;
using DPA_Musicsheets.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Factories
{
    static class LilypondFactory
    {
        public static string GetLilypond(Composition composition)
        {
            ToLilypondVisitor visitor = new ToLilypondVisitor();
            foreach (var token in composition.Tokens)
            {
                token.Accept(visitor);
            }
            return visitor.Build();
        }
    }
}
