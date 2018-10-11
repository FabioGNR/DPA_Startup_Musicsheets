﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Models.Domain;

namespace DPA_Musicsheets.Converters
{
    class ToLilypondVisitor : ITokenVisitor
    {
        private StringBuilder lilypondText = new StringBuilder();
        private Note lastNote = null;
        private int barsPerLine = 2;
        private int currentBarsOnLine = 0;

        public string Build() => lilypondText.ToString();

        public void ProcessToken(Note note)
        {
            int octaveOffset = note.Pitch.OctaveOffset;
            if (lastNote != null)
            {
                // relative offset
                octaveOffset = lastNote.Pitch.OctaveOffset - octaveOffset;
            }
            lilypondText.Append(note.Pitch.Tone.ToString().ToLower());
            if (note.Pitch.Accidental != Accidental.None)
            {
                lilypondText.Append(note.Pitch.Accidental == Accidental.Sharp ? "is" : "es");
            }
            AppendOctaveOffset(octaveOffset);
            AppendLength(note.Length);
            lilypondText.Append(' ');
            lastNote = note;
        }

        public void ProcessToken(Rest rest)
        {
            lilypondText.Append('r');
            AppendLength(rest.Length);
            lilypondText.Append(' ');
        }

        public void ProcessToken(Barline barLine)
        {
            currentBarsOnLine++;
            lilypondText.Append("| ");
            if (currentBarsOnLine >= barsPerLine)
            {
                currentBarsOnLine = 0;
                lilypondText.AppendLine();
            }
        }

        public void ProcessToken(TimeSignature timeSignature)
        {
            lilypondText.Append(@"\time ");
            lilypondText.Append(timeSignature.Count);
            lilypondText.Append('/');
            lilypondText.Append(timeSignature.Denominator);
            lilypondText.AppendLine();
        }

        public void ProcessToken(Tempo tempo)
        {
            //TODO: read 4 from Denominator in TimeSignature
            lilypondText.Append(@"\tempo 4=");
            lilypondText.Append(tempo.BPM);
            lilypondText.AppendLine();
        }

        public void ProcessToken(Clef clef)
        {
            lilypondText.Append(@"\clef ");
            lilypondText.Append(clef.Tone);
            lilypondText.AppendLine();
        }

        public void ProcessToken(Token any)
        {
            throw new NotSupportedException($"{any.GetType()} is not supported in this visitor");
        }

        private void AppendLength(Length length)
        {
            lilypondText.Append(length.Denominator);
            lilypondText.Append('.', length.AmountOfDots);
        }

        private void AppendOctaveOffset(int octaveOffset)
        {
            if (octaveOffset > 0)
            {
                lilypondText.Append('\'', octaveOffset);
            }
            else
            {
                lilypondText.Append(',', -octaveOffset);
            }
        }
    }
}