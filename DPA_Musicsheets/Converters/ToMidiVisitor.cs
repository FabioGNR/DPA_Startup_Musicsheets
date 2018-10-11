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

        public void ProcessToken(Note note)
        {
            throw new NotImplementedException();
        }

        public void ProcessToken(Rest rest)
        {
            throw new NotImplementedException();
        }

        public void ProcessToken(Barline barLine)
        {

        }

        public void ProcessToken(TimeSignature timeSignature)
        {
            byte[] messageData = new byte[2];
            messageData[0] = Convert.ToByte(timeSignature.Count);
            messageData[1] = Convert.ToByte(Math.Log(timeSignature.Denominator.Value, 2));
            var metaMessage = new MetaMessage(MetaType.TimeSignature, messageData);
            metaTrack.Insert(currentAbsoluteTicks, metaMessage);
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

        private int GetTicksForLength(Length length)
        {
            // TODO: calculate amount of ticks for length
            return 0;
        }
    }
}
