using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Models.Domain;

namespace DPA_Musicsheets.Converters
{
    class InsertBarCompositionHandler : BaseCompositionHandler, ITokenVisitor
    {
        private TimeSignature timeSignature;
        private decimal barPercentage;
        private Composition composition;
        private int index;


        public void ProcessToken(Note note)
        {
            CheckForBarline(note.Length);
        }

        public void ProcessToken(Rest rest)
        {
            CheckForBarline(rest.Length);
        }

        public void ProcessToken(Barline barLine)
        {
            barPercentage = 0;
        }

        public void ProcessToken(TimeSignature timeSignature)
        {
            this.timeSignature = timeSignature;
            barPercentage = 0;
        }

        public void ProcessToken(Tempo tempo)
        {
            // do nothing
        }

        public void ProcessToken(Clef clef)
        {
            // do nothing
        }

        public void ProcessToken(Token any)
        {
            // do nothing
        }

        private void CheckForBarline(Length length)
        {
            decimal fullBar = (decimal)timeSignature.Count / timeSignature.Denominator.Value;
            int denom = length.Denominator.Value;
            decimal noteLength = 1m / denom * (2m - 1m / (decimal)Math.Pow(2, length.AmountOfDots));
            barPercentage += noteLength / fullBar;
            if (barPercentage >= 1) InsertBarline();
        }

        private void InsertBarline()
        {
            composition.Tokens.Insert(index + 1, new Barline());
        }

        protected override void TryHandle(Composition composition)
        {
            this.composition = composition;
            IList<Token> tokens = composition.Tokens;
            for (index = 0; index < composition.Tokens.Count; index++)
            {
                tokens[index].Accept(this);
            }
        }
    }
}
