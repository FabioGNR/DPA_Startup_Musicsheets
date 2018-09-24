using DPA_Musicsheets.Builders;
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
        private SymbolBuilder currentSymbolBuilder = null;

        public MidiComposer(Sequence sequence)
        {
            this.sequence = sequence;
            events = JoinTracks(sequence);
        }

        public Composition Compose()
        {
            // for midi always use G Clef
            composition.Tokens.Add(
                new ClefBuilder()
                .WithTone(ClefTone.G)
                .OnBar(2)
                .Build());
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

        private void ProcessMetaMessage(MidiEvent evt)
        {
            MetaMessage msg = evt.MidiMessage as MetaMessage;
            byte[] msgBytes = msg.GetBytes();
            switch (msg.MetaType)
            {
                case MetaType.TimeSignature:
                    int count = msgBytes[0];
                    int denominator = (int)Math.Pow(msgBytes[1], 2);
                    Builders.TimeSignatureBuilder timeSignatureBuilder = new Builders.TimeSignatureBuilder();
                    timeSignatureBuilder.WithCount(count);
                    timeSignatureBuilder.WithDenominator(denominator);
                    currentTimeSignature = timeSignatureBuilder.Build();
                    composition.Tokens.Add(currentTimeSignature);
                    break;
                case MetaType.Tempo:
                    int microSecondsPB = (msgBytes[0] << 16 | msgBytes[1] << 8 | msgBytes[2]);
                    int BPM = 60_000_000 / microSecondsPB;
                    composition.Tokens.Add(new TempoBuilder().WithBPM(BPM).Build());
                    break;
            }
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
                    AddDefaultTimeSignature();
                }
                if (evt.DeltaTicks > 0)
                {
                    InsertRest(evt.DeltaTicks);
                }
                AddPitchToSymbol(msg.Data1);
            }
            else if (currentSymbolBuilder != null)
            {
                EndCurrentNote(evt);
            }
        }

        private void InsertRest(int deltaTicks)
        {
            SymbolBuilder symbolBuilder = new SymbolBuilder();
            symbolBuilder = AddLengthToSymbol(deltaTicks, symbolBuilder);
            Symbol rest = symbolBuilder.Build();
            composition.Tokens.Add(rest);
            CheckForBar(rest.Length);
        }

        private void AddDefaultTimeSignature()
        {
            Builders.TimeSignatureBuilder timeSignatureBuilder = new Builders.TimeSignatureBuilder();
            timeSignatureBuilder.WithCount(4);
            timeSignatureBuilder.WithDenominator(4);
            currentTimeSignature = timeSignatureBuilder.Build();
            composition.Tokens.Add(currentTimeSignature);
        }

        private void CheckForBar(Length length)
        {
            int denom = length.Denominator.Value;
            currentBarPercentage += 1m / denom * (2m - 1m / (decimal)Math.Pow(2, length.AmountOfDots));
            if (currentBarPercentage >= (decimal)currentTimeSignature.Count / currentTimeSignature.Denominator.Value)
            {
                composition.Tokens.Add(new Barline());
                currentBarPercentage = 0;
            }

        }

        private void EndCurrentNote(MidiEvent evt)
        {
            AddLengthToSymbol(evt.DeltaTicks, currentSymbolBuilder);
            Symbol note = currentSymbolBuilder.Build();
            composition.Tokens.Add(note);
            currentSymbolBuilder = null;
            CheckForBar(note.Length);
        }

        private SymbolBuilder AddLengthToSymbol(int deltaTicks, SymbolBuilder builder)
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
            if (builder == null)
                builder = new SymbolBuilder();
            builder.WithLength(denominator, amountOfDots);
            return builder;
        }

        private void AddPitchToSymbol(int keyCode)
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

            if (currentSymbolBuilder == null)
                currentSymbolBuilder = new SymbolBuilder();
            currentSymbolBuilder.WithPitch(tone, acc, GetOctaveOffset(keyCode));
        }

        private int GetOctaveOffset(int keyCode)
        {
            return (int)Math.Floor((keyCode - CENTRAL_C) / 12f);
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
