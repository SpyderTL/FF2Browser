using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperNintendo;

namespace FinalFantasy2
{
	public static class ChannelReader
	{
		public static int Position;
		public static int Value;
		public static EventTypes EventType;
		public static int Special;
		public static int Note;
		public static int Octave;
		public static int Length;
		public static int Duration;
		public static int Velocity;
		public static int Call;
		public static int Jump;
		public static int Loop;
		public static int Repeat;
		public static int Tempo;
		public static int Pan;
		public static int Phase;
		public static int Instrument;
		public static int PercussionInstrumentOffset;
		public static int Volume;
		public static int Fade;
		public static int Transpose;
		public static int Tuning;
		public static int PitchSlide;
		public static int Delay;

		public static void Read()
		{
			Value = Apu.Memory[Position++];

			if (Value <= Game.LastNote)
			{
				EventType = EventTypes.Note;
				Note = Value / 15;
				Duration = Value % 15;
			}
			else if (Value <= Game.LastRest)
			{
				EventType = EventTypes.Rest;
				Duration = Value - Game.FirstRest;
			}
			else if (Value <= Game.LastTie)
			{
				EventType = EventTypes.Tie;
				Duration = Value - Game.FirstTie;
			}
			else
			{
				switch (Value)
				{
					case 0xD2:
						EventType = EventTypes.Tempo;
						Fade = Apu.Memory[Position++] | (Apu.Memory[Position++] << 8);
						Tempo = Apu.Memory[Position++];
						break;

					case 0xD3:
						EventType = EventTypes.Other;
						Position += 3;
						break;

					case 0xD4:
						EventType = EventTypes.Other;
						Position += 1;
						break;

					case 0xD5:
						EventType = EventTypes.Other;
						Position += 2;
						break;

					case 0xD6:
						EventType = EventTypes.Other;
						Position += 3;
						break;

					case 0xD7:
						EventType = EventTypes.Other;
						Position += 3;
						break;

					case 0xD8:
						EventType = EventTypes.Other;
						Position += 3;
						break;

					case 0xD9:
						EventType = EventTypes.Other;
						Position += 3;
						break;

					case 0xDA:
						EventType = EventTypes.Octave;
						Octave = Apu.Memory[Position++];
						break;

					case 0xDB:
						EventType = EventTypes.Instrument;
						Instrument = Apu.Memory[Position++];
						break;

					case 0xDC:
						EventType = EventTypes.Other;
						Position += 1;
						break;

					case 0xDD:
						EventType = EventTypes.Other;
						Position += 1;
						break;

					case 0xDE:
						EventType = EventTypes.Other;
						Volume = Apu.Memory[Position++];
						System.Diagnostics.Debug.WriteLine("Volume? " + Volume);
						break;

					case 0xDF:
						EventType = EventTypes.Other;
						Position += 1;
						break;

					case 0xE0:
						EventType = EventTypes.LoopStart;
						Repeat = Apu.Memory[Position++];
						Loop = Position;
						break;

					case 0xE1:
						EventType = EventTypes.OctaveUp;
						break;

					case 0xE2:
						EventType = EventTypes.OctaveDown;
						break;

					case 0xEB:
						EventType = EventTypes.Other;
						break;

					case 0xEC:
						EventType = EventTypes.Other;
						break;

					case 0xF0:
						EventType = EventTypes.LoopEnd;
						break;

					case 0xF2:
						EventType = EventTypes.Volume;
						Duration = Apu.Memory[Position++] | (Apu.Memory[Position++] << 8);
						Volume = Apu.Memory[Position++];
						System.Diagnostics.Debug.WriteLine("Volume: " + Volume + " " + Duration.ToString("X4"));
						break;

					case 0xF3:
						EventType = EventTypes.Other;
						Position += 3;
						break;

					case 0xF4:
						EventType = EventTypes.Jump;
						Jump = Apu.Memory[Position++] | (Apu.Memory[Position++] << 8);
						break;

					case 0xF5:
						EventType = EventTypes.LoopExit;
						Loop = Apu.Memory[Position++];
						Jump = Apu.Memory[Position++] | (Apu.Memory[Position++] << 8);
						//Position += 3;
						break;

					case 0xF1:
					case 0xF6:
					case 0xF7:
					case 0xF8:
					case 0xF9:
					case 0xFA:
					case 0xFB:
					case 0xFC:
					case 0xFD:
					case 0xFE:
					case 0xFF:
						EventType = EventTypes.Stop;
						break;

					default:
						EventType = EventTypes.Other;
						break;
				}
			}
		}

		public enum EventTypes
		{
			Note,
			Tie,
			Rest,
			Length,
			LengthDurationVelocity,
			LengthDuration,
			LengthVelocity,
			Pan,
			PanFade,
			Percussion,
			Tempo,
			TempoFade,
			Call,
			Other,
			Stop,
			Instrument,
			PercussionInstrumentOffset,
			Volume,
			VolumeFade,
			Transpose,
			Tuning,
			PitchSlideFrom,
			PitchSlideTo,
			PitchSlideOff,
			MasterVolume,
			MasterVolumeFade,
			TremoloOn,
			TremoloOff,
			Echo,
			VibratoOn,
			VibratoOff,
			GlobalTranspose,
			LoopStart,
			LoopEnd,
			LoopExit,
			Octave,
			OctaveUp,
			OctaveDown,
			Jump,
		}
	}
}
