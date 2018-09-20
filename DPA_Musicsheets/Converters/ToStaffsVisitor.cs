using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Models.Domain;

namespace DPA_Musicsheets.Converters
{
    public class ToStaffsVisitor : ITokenVisitor
    {
        private readonly IList<PSAMControlLibrary.MusicalSymbol> _symbols =
            new List<PSAMControlLibrary.MusicalSymbol>();

        public IEnumerable<PSAMControlLibrary.MusicalSymbol> Symbols => _symbols.AsEnumerable();

        private int GetAlterFromPitch(Pitch pitch)
        {
            var accidental = pitch.Accidental;
            switch (accidental)
            {
                case Accidental.Sharp:
                    return 1;
                case Accidental.Flat:
                    return -1;
                default:
                    return 0;
            }
        }

        private PSAMControlLibrary.MusicalSymbolDuration ToDuration(Length length)
        {
            return (PSAMControlLibrary.MusicalSymbolDuration)length.Denominator.Value;
        }

        public void ProcessToken(Note note)
        {
            var staffNote = new PSAMControlLibrary.Note(
                note.Pitch.Tone.ToString(),
                GetAlterFromPitch(note.Pitch),
                note.Pitch.OctaveOffset + 4,
                ToDuration(note.Length),
                PSAMControlLibrary.NoteStemDirection.Up,
                PSAMControlLibrary.NoteTieType.None,
                new List<PSAMControlLibrary.NoteBeamType>()
                {
                    PSAMControlLibrary.NoteBeamType.Single
                }
            );
            _symbols.Add(staffNote);
        }

        public void ProcessToken(Rest rest)
        {
            var staffRest = new PSAMControlLibrary.Rest(
                            ToDuration(rest.Length));
            _symbols.Add(staffRest);
        }

        public void ProcessToken(Barline barline)
        {
            var staffBarline = new PSAMControlLibrary.Barline();
            _symbols.Add(staffBarline);
        }

        public void ProcessToken(TimeSignature timeSignature)
        {
            var staffTimeSignature = new PSAMControlLibrary.TimeSignature(
                PSAMControlLibrary.TimeSignatureType.Numbers,
                Convert.ToUInt32(timeSignature.Count),
                Convert.ToUInt32(timeSignature.Denominator.Value));
            _symbols.Add(staffTimeSignature);
        }

        public void ProcessToken(Tempo tempo)
        {

        }

        public void ProcessToken(Token any)
        {

        }
    }
}
