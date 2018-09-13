using DPA_Musicsheets.Models.Domain;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Factories
{
    public class MidiComposer
    {

        private const int CENTRAL_C = 60;

        private readonly Sequence sequence;
        private readonly IEnumerable<MidiEvent> events;
        private readonly Composition composition = new Composition();

        private TimeSignature currentTimeSignature;

        private Note currentNote = null;
        private long lastNoteEventTicks = 0;

        public MidiComposer(Sequence sequence)
        {
            this.sequence = sequence;
            events = JoinTracks(sequence);
        }

        public Composition Compose()
        {
            foreach (var evt in events)
            {
                switch (evt.MidiMessage.MessageType)
                {
                    case MessageType.Meta:
                        ProcessMetaMessage(evt);
                        break;
                    case MessageType.Channel:
                        ProcessChannelMessage(evt);
                        break;
                    default:
                        break;
                }
            }
            return composition;
        }

        private void ProcessChannelMessage(MidiEvent evt)
        {
            ChannelMessage msg = evt.MidiMessage as ChannelMessage;
            if (msg == null || msg.Command != ChannelCommand.NoteOn)
                return;
            if (msg.Data2 > 0)
            {
                Pitch pitch = new Pitch();
                currentNote = new Note
                {
                    Pitch = GetPitch(msg.Data1)
                };
            }
            else if (currentNote != null)
            {
                EndCurrentNote(evt);
            }
        }

        private void EndCurrentNote(MidiEvent evt)
        {
            Length length = CalculateLength(evt.DeltaTicks);
            currentNote.Length = length;
            composition.Tokens.Add(currentNote);
            currentNote = null;
        }

        private Length CalculateLength(int deltaTicks)
        {
            double amountOfBeatNotes = (double)deltaTicks / (double)sequence.Division;
            double amountOfFullNotes = amountOfBeatNotes / currentTimeSignature.Denominator.Value;

            int n = 0;
            int denominator = (int)Math.Pow(2, n);
            while (1f / denominator > amountOfFullNotes)
            {
                n++;
                denominator = (int)Math.Pow(2, n);
            }
            double baseNoteLength = 1f / denominator;
            int amountOfDots = Math.Min(4, (int)Math.Log(-baseNoteLength / (amountOfFullNotes - 2 * baseNoteLength), 2));
            Length length = new Length
            {
                Denominator = new Denominator(denominator),
                AmountOfDots = amountOfDots
            };

            return length;
        }

        private Pitch GetPitch(int keyCode)
        {
            Tone tone;
            Accidental acc = Accidental.None;

            int toneNumber = keyCode % 12;
            switch (toneNumber)
            {
                case 0:
                    tone = Tone.C;
                    break;
                case 1:
                    tone = Tone.C;
                    acc = Accidental.Sharp;
                    break;
                case 2:
                    tone = Tone.D;
                    break;
                case 3:
                    tone = Tone.D;
                    acc = Accidental.Sharp;
                    break;
                case 4:
                    tone = Tone.E;
                    break;
                case 5:
                    tone = Tone.F;
                    break;
                case 6:
                    tone = Tone.F;
                    acc = Accidental.Sharp;
                    break;
                case 7:
                    tone = Tone.G;
                    break;
                case 8:
                    tone = Tone.G;
                    acc = Accidental.Sharp;
                    break;
                case 9:
                    tone = Tone.A;
                    break;
                case 10:
                    tone = Tone.A;
                    acc = Accidental.Sharp;
                    break;
                case 11:
                    tone = Tone.B;
                    break;
                default:
                    throw new ArgumentException("Tone does not exist", nameof(keyCode));
            }
            return new Pitch
            {
                Tone = tone,
                Accidental = acc,
                OctaveOffset = GetOctaveOffset(keyCode)
            };
        }

        private int GetOctaveOffset(int keyCode)
        {
            return (int)Math.Floor((keyCode - CENTRAL_C) / 12f);
        }

        private void ProcessMetaMessage(MidiEvent evt)
        {
            MetaMessage msg = evt.MidiMessage as MetaMessage;
            byte[] msgBytes = msg.GetBytes();
            switch (msg.MetaType)
            {
                case MetaType.TimeSignature:
                    int count = msgBytes[0];
                    int denominator = (int)Math.Pow(msgBytes[1], 2);
                    currentTimeSignature = new TimeSignature
                    {
                        Count = count,
                        Denominator = new Denominator(denominator)
                    };
                    composition.Tokens.Add(currentTimeSignature);
                    break;
                case MetaType.Tempo:
                    int microSecondsPB = (msgBytes[0] << 16 | msgBytes[1] << 8 | msgBytes[2]);
                    composition.Tokens.Add(new Tempo(60_000_000 / microSecondsPB));
                    break;
                case MetaType.EndOfTrack:
                    if (currentNote != null)
                    {
                        EndCurrentNote(evt);
                    }
                    break;
            }
        }

        private IEnumerable<MidiEvent> JoinTracks(Sequence sequence)
        {
            IEnumerable<MidiEvent> events = new List<MidiEvent>();
            for (int i = 0; i < Math.Min(2, sequence.Count); i++)
            {
                events = events.Concat(sequence[i].Iterator());
            }
            // sort based on time
            return events.OrderBy(evt => evt.AbsoluteTicks);
        }
    }
}
