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
        private const int TONES_IN_OCTAVE = 12;

        private readonly Sequence sequence;
        private readonly IEnumerable<MidiEvent> events;
        private readonly Composition composition = new Composition();
        private decimal currentBarPercentage = 0;

        private TimeSignature currentTimeSignature;
        private Note currentNote = null;

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
                if (currentTimeSignature == null)
                {
                    currentTimeSignature = new TimeSignature
                    {
                        Count = 4,
                        Denominator = new Denominator(4)
                    };
                    composition.Tokens.Add(currentTimeSignature);
                }
                if (evt.DeltaTicks > 0)
                {
                    Length length = CalculateLength(evt.DeltaTicks);
                    Rest rest = new Rest
                    {
                        Length = length
                    };
                    composition.Tokens.Add(rest);
                    CheckForBar(length);

                }
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

        private void CheckForBar(Length length)
        {
            int denom = length.Denominator.Value;
            currentBarPercentage += 1m / denom * (2m - 1m / (decimal)Math.Pow(2, length.AmountOfDots));
            if (currentBarPercentage >= (decimal)currentTimeSignature.Count / currentTimeSignature.Denominator.Value)
            {
                composition.Tokens.Add(new BarLine());
                currentBarPercentage = 0;
            }

        }

        private void EndCurrentNote(MidiEvent evt)
        {
            Length length = CalculateLength(evt.DeltaTicks);
            currentNote.Length = length;
            composition.Tokens.Add(currentNote);
            currentNote = null;
            CheckForBar(length);
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
            Tone tone = Tone.C;
            Accidental acc = Accidental.None;

            int toneNumber = keyCode % TONES_IN_OCTAVE;

            var values = Enum.GetValues(typeof(Tone)).Cast<int>();
            for (int i = 0; i <= toneNumber; i++)
            {

                if (values.Contains(i))
                {
                    tone = (Tone)i;
                    acc = Accidental.None;
                }
                else
                {
                    acc = Accidental.Sharp;
                }
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
            return events.OrderBy(evt => evt.AbsoluteTicks).ThenByDescending(evt => evt.DeltaTicks);
        }
    }
}
