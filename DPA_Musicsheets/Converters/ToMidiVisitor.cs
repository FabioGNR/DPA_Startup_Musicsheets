using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Models.Domain;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.Converters
{
    public class ToMidiVisitor : ITokenVisitor
    {
        private TimeSignature currentTimeSignature = null;
        private Sequence sequence;
        private Track metaTrack;
        private Track channelTrack;
        private int currentAbsoluteTicks = 0;

        public ToMidiVisitor()
        {
            sequence = new Sequence();
            metaTrack = new Track();
            sequence.Add(metaTrack);
            channelTrack = new Track();
            sequence.Add(channelTrack);
        }

        public Sequence Sequence => sequence;

        public void ProcessToken(Note note)
        {
            //determine key
            int keyNumber = GetKeyForPitch(note.Pitch);
            //determine length in ticks
            int deltaTicks = GetTicksForLength(note.Length);
            //insert note on
            channelTrack.Insert(currentAbsoluteTicks, new ChannelMessage(ChannelCommand.NoteOn, 1, keyNumber, 90)); // Data2 = volume
            currentAbsoluteTicks += deltaTicks;
            //insert note off
            channelTrack.Insert(currentAbsoluteTicks, new ChannelMessage(ChannelCommand.NoteOn, 1, keyNumber, 0)); // Data2 = volume
        }

        public void ProcessToken(Rest rest)
        {
            int deltaTicks = GetTicksForLength(rest.Length);
            currentAbsoluteTicks += deltaTicks;
        }

        public void ProcessToken(Barline barLine)
        {
            // do nothing
        }

        public void ProcessToken(TimeSignature timeSignature)
        {
            byte[] messageData = new byte[4];
            messageData[0] = Convert.ToByte(timeSignature.Count);
            messageData[1] = Convert.ToByte(Math.Log(timeSignature.Denominator.Value, 2));
            var metaMessage = new MetaMessage(MetaType.TimeSignature, messageData);
            metaTrack.Insert(currentAbsoluteTicks, metaMessage);
            currentTimeSignature = timeSignature;
        }

        public void ProcessToken(Tempo tempo)
        {
            int microsecondsPerPB = Convert.ToInt32((60_000_000 / tempo.BPM));
            byte[] messageData = new byte[3];
            messageData[0] = (byte)((microsecondsPerPB >> 16) & 0xff);
            messageData[1] = (byte)((microsecondsPerPB >> 8) & 0xff);
            messageData[2] = (byte)(microsecondsPerPB & 0xff);
            var metaMessage = new MetaMessage(MetaType.Tempo, messageData);
            metaTrack.Insert(currentAbsoluteTicks, metaMessage);
        }

        public void ProcessToken(Clef clef)
        {
            // do nothing
        }

        public void ProcessToken(Token any)
        {
            // do nothing
        }

        private int GetKeyForPitch(Pitch pitch)
        {
            int keyPosition = (int)pitch.Tone;
            if (pitch.Accidental != Accidental.None)
            {
                keyPosition += pitch.Accidental == Accidental.Sharp ? 1 : -1;
            }
            int keyNumber = Constants.MIDI.CENTRAL_C + (pitch.OctaveOffset * Constants.TONES_IN_OCTAVE) + keyPosition;
            return keyNumber;
        }

        private int GetTicksForLength(Length length)
        {
            // Calculate duration
            double absoluteLength = 1.0 / (double)length.Denominator.Value;
            absoluteLength += (absoluteLength / 2.0) * length.AmountOfDots;

            double relationToQuartNote = currentTimeSignature.Denominator.Value / 4.0;
            double percentageOfBeatNote = (1.0 / currentTimeSignature.Denominator.Value) / absoluteLength;
            double deltaTicks = (sequence.Division / relationToQuartNote) / percentageOfBeatNote;
            return (int)deltaTicks;
        }
    }
}
