using DPA_Musicsheets.Builders;
using DPA_Musicsheets.Models.Domain;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace DPA_Musicsheets.Converters.Lilypond
{
    internal class LilypondNoteConverter : ILilypondTokenConverter
    {
        private static Regex noteRegex = new Regex(@"(~?)([a-g])(is|es)?([,']*)(\d+)(\.*)");
        private int octaveOffset = 0;
        private Tone? previousNoteTone = null;

        public Token Convert(LilypondTokenEnumerator enumerator)
        {
            var input = enumerator.Current;
            var match = noteRegex.Match(input.TokenText);
            if (match.Success)
            {
                SymbolBuilder builder = new SymbolBuilder();
                bool slur = match.Groups[0].Success;
                //TODO: add slur to builder
                var tone = GetToneFromLetter(match.Groups[2].Value);
                Accidental accidental = Accidental.None;
                if (match.Groups[3].Success)
                {
                    accidental = match.Groups[3].Value == "is" ? Accidental.Sharp : Accidental.Flat;
                }
                // determine octave
                MoveToClosestOctaveOffset(tone);
                previousNoteTone = tone;
                if (match.Groups[4].Success)
                {
                    UpdateOctaveOffset(match.Groups[4].Value);
                }
                // set pitch
                builder.WithPitch(tone, accidental, octaveOffset);
                // determine length
                if (int.TryParse(match.Groups[5].Value, out int denominator))
                {
                    int dots = match.Groups[6].Length;
                    builder.WithLength(denominator, dots);
                }
                return builder.Build();
            }
            else
            {
                throw new InvalidOperationException("Not a note that can be read");
            }
        }

        /// <summary>
        /// This method adjusts the octave offset so that the note is closest to the previous
        /// </summary>
        /// <param name="newTone"></param>
        private void MoveToClosestOctaveOffset(Tone newTone)
        {
            if (previousNoteTone == null)
            {
                return;
            }
            int toneValue = (int)newTone;
            int previousToneValue = (int)previousNoteTone;
            if (previousToneValue > toneValue && previousToneValue - toneValue > 6)
            {
                octaveOffset++;
            }
            else if (toneValue - previousToneValue > 6)
            {
                octaveOffset--;
            }
        }

        private void UpdateOctaveOffset(string octaveModifiers)
        {
            int octaveIncreases = octaveModifiers.Count(c => c == '\'');
            int octaveDecreases = octaveModifiers.Count(c => c == ',');
            octaveOffset += octaveIncreases - octaveDecreases;
        }

        private Tone GetToneFromLetter(string letter)
        {
            if (Enum.TryParse(value: letter, ignoreCase: true, result: out Tone tone))
            {
                return tone;
            }
            else
            {
                throw new NotSupportedException("This note was not recognized");
            }
        }
    }
}