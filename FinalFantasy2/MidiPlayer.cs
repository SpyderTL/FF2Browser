using System;
using System.Diagnostics;
using System.Linq;

namespace FinalFantasy2
{
	internal static class MidiPlayer
	{
		internal static int Song;
		internal static int[] ChannelNote;
		internal static int[] ChannelInstruments;
		internal static int[] ChannelOffset;
		internal static int[] ChannelDrums;

		internal static void Start()
		{
			ChannelNote = Enumerable.Repeat(-1, 8).ToArray();
			ChannelInstruments = Enumerable.Repeat(-1, 8).ToArray();
			ChannelOffset = new int[8];
			ChannelDrums = Enumerable.Repeat(-1, 8).ToArray();

			Midi.Midi.Enable();

			for (var channel = 0; channel < 8; channel++)
			{
				Midi.Midi.ControlChange(channel, 123, 0);
				Midi.Midi.ProgramChange(channel, Midi.Midi.Patches.GrandPiano);
				Midi.Midi.ControlChange(channel, Midi.Midi.Controls.Reverb, 127);
				Midi.Midi.ControlChange(channel, Midi.Midi.Controls.Tremolo, 127);
				Midi.Midi.ControlChange(channel, Midi.Midi.Controls.Chorus, 127);
				Midi.Midi.ControlChange(channel, Midi.Midi.Controls.Detune, 127);
				Midi.Midi.ControlChange(channel, Midi.Midi.Controls.Phaser, 127);
			}
		}

		internal static void Stop()
		{
			for (var channel = 0; channel < 8; channel++)
				Midi.Midi.ControlChange(channel, 123, 0);

			Midi.Midi.Disable();
		}

		internal static void Update()
		{
			for (var channel = 0; channel < SongPlayer.ChannelTriggers.Length; channel++)
			{
				if (SongPlayer.ChannelInstruments[channel] != ChannelInstruments[channel])
				{
					ChangeInstrument(channel);

					ChannelInstruments[channel] = SongPlayer.ChannelInstruments[channel];
				}

				if (SongPlayer.ChannelNotes[channel] == -1)
				{
					if (ChannelNote[channel] != -1)
					{
						Midi.Midi.NoteOff(channel, ChannelNote[channel], 0);
						ChannelNote[channel] = -1;
					}
				}
				else if (SongPlayer.ChannelTriggers[channel])
				{
					if (ChannelNote[channel] != -1)
						Midi.Midi.NoteOff(channel, ChannelNote[channel], 0);

					if (ChannelDrums[channel] == -1)
					{
						Midi.Midi.NoteOn(channel, SongPlayer.ChannelNotes[channel] + (SongPlayer.ChannelOctaves[channel] * 12) + ChannelOffset[channel], (SongPlayer.ChannelVolumes[channel] >> 2) + 64);
						ChannelNote[channel] = SongPlayer.ChannelNotes[channel] + (SongPlayer.ChannelOctaves[channel] * 12) + ChannelOffset[channel];
					}
					else
					{
						// Drums
						Midi.Midi.NoteOn(9, ChannelDrums[channel], (SongPlayer.ChannelVolumes[channel] >> 2) + 64);
						Midi.Midi.NoteOff(9, ChannelDrums[channel], 0);
					}
				}
			}
		}

		private static void ChangeInstrument(int channel)
		{
			var instrument = Midi.Midi.Patches.GrandPiano;
			var offset = 0;
			var drums = -1;

			switch (Song)
			{
				case 1:
					switch (SongPlayer.ChannelInstruments[channel])
					{
						case 0x40:
							instrument = Midi.Midi.Patches.Strings;
							offset = 12;
							break;

						case 0x41:
							instrument = Midi.Midi.Patches.Trumpet;
							break;

						case 0x42:
							instrument = Midi.Midi.Patches.GrandPiano;
							break;

						case 0x43:
							drums = Midi.Midi.Drums.CrashCymbal;
							break;

						case 0x44:
							instrument = Midi.Midi.Patches.Timpani;
							offset = -12;
							break;

						case 0x45:
							instrument = Midi.Midi.Patches.Strings;
							offset = 12;
							break;

						default:
							instrument = Midi.Midi.Patches.SquareLead;
							break;
					}
					break;

				case 11:
					switch (SongPlayer.ChannelInstruments[channel])
					{
						case 0x40:
							instrument = Midi.Midi.Patches.Timpani;
							offset = -24;
							break;

						case 0x41:
							instrument = Midi.Midi.Patches.Trombone;
							offset = 0;
							break;

						case 0x42:
							instrument = Midi.Midi.Patches.PickBass;
							offset = -24;
							break;

						case 0x43:
							drums = Midi.Midi.Drums.CrashCymbal;
							break;

						case 0x44:
							instrument = Midi.Midi.Patches.Strings;
							offset = 12;
							break;

						case 0x45:
							drums = Midi.Midi.Drums.HiHat;
							break;

						default:
							instrument = Midi.Midi.Patches.GrandPiano;
							break;
					}
					break;

				case 19:
					switch (SongPlayer.ChannelInstruments[channel])
					{
						case 0x40:
							instrument = Midi.Midi.Patches.Flute;
							offset = 12;
							break;

						case 0x41:
							//instrument = Midi.Midi.Patches.PizzicatoStrings;
							instrument = Midi.Midi.Patches.Harp;
							offset = 12;
							break;

						case 0x42:
							instrument = Midi.Midi.Patches.Strings;
							offset = 12;
							break;

						default:
							instrument = Midi.Midi.Patches.GrandPiano;
							break;
					}
					break;

				case 21:
					switch (SongPlayer.ChannelInstruments[channel])
					{
						case 0x40:
							instrument = Midi.Midi.Patches.Harp;
							offset = 12;
							break;

						case 0x41:
							instrument = Midi.Midi.Patches.Strings;
							offset = 12;
							break;

						case 0x42:
							instrument = Midi.Midi.Patches.Flute;
							offset = 12;
							break;

						default:
							instrument = Midi.Midi.Patches.GrandPiano;
							break;
					}
					break;

				case 34:
					switch (SongPlayer.ChannelInstruments[channel])
					{
						case 0x40:
							instrument = Midi.Midi.Patches.Flute;
							offset = 12;
							break;

						case 0x41:
							instrument = Midi.Midi.Patches.ChurchOrgan;
							break;

						case 0x42:
							instrument = Midi.Midi.Patches.FingerBass;
							offset = -24;
							break;

						case 0x43:
							drums = Midi.Midi.Drums.SnareDrum;
							break;

						default:
							instrument = Midi.Midi.Patches.GrandPiano;
							break;
					}
					break;

				case 39:
					switch (SongPlayer.ChannelInstruments[channel])
					{
						case 0x40:
							instrument = Midi.Midi.Patches.Trumpet;
							break;

						case 0x41:
							instrument = Midi.Midi.Patches.SynthStrings;
							offset = 12;
							break;

						case 0x42:
							instrument = Midi.Midi.Patches.FingerBass;
							offset = -24;
							break;

						case 0x43:
							drums = Midi.Midi.Drums.CrashCymbal;
							break;

						case 0x44:
							drums = Midi.Midi.Drums.BassDrum;
							break;

						case 0x45:
							drums = Midi.Midi.Drums.SnareDrum;
							break;

						case 0x46:
							drums = Midi.Midi.Drums.HiHat;
							break;

						default:
							instrument = Midi.Midi.Patches.GrandPiano;
							break;
					}
					break;

				case 44:
					switch (SongPlayer.ChannelInstruments[channel])
					{
						case 0x40:
							//instrument = Midi.Midi.Patches.Trumpet;
							instrument = Midi.Midi.Patches.BrassSection;
							break;

						case 0x41:
							instrument = Midi.Midi.Patches.Strings;
							offset = 12;
							break;

						case 0x42:
							instrument = Midi.Midi.Patches.FingerBass;
							offset = -24;
							break;

						case 0x43:
							drums = Midi.Midi.Drums.SnareDrum;
							break;

						case 0x44:
							drums = Midi.Midi.Drums.BassDrum;
							break;

						case 0x45:
							drums = Midi.Midi.Drums.CrashCymbal;
							break;

						case 0x46:
							drums = Midi.Midi.Drums.HiHat;
							break;

						default:
							instrument = Midi.Midi.Patches.GrandPiano;
							break;
					}
					break;

				case 53:
					switch (SongPlayer.ChannelInstruments[channel])
					{
						case 0x40:
							instrument = Midi.Midi.Patches.FingerBass;
							offset = -24;
							break;

						case 0x41:
							instrument = Midi.Midi.Patches.Trumpet;
							break;

						case 0x42:
							instrument = Midi.Midi.Patches.Strings;
							offset = 12;
							break;

						case 0x43:
							drums = Midi.Midi.Drums.OpenHiHat;
							break;

						case 0x44:
							drums = Midi.Midi.Drums.BassDrum;
							break;

						case 0x45:
							drums = Midi.Midi.Drums.SnareDrum;
							break;

						case 0x46:
							//instrument = Midi.Midi.Patches.PizzicatoStrings;
							instrument = Midi.Midi.Patches.Harp;
							offset = 12;
							break;

						case 0x47:
							instrument = Midi.Midi.Patches.Flute;
							offset = 12;
							break;

						case 0x48:
							drums = Midi.Midi.Drums.CrashCymbal;
							break;

						case 0x49:
							instrument = Midi.Midi.Patches.GrandPiano;
							offset = -12;
							break;

						default:
							instrument = Midi.Midi.Patches.SquareLead;
							break;
					}
					break;

				case 54:
					switch (SongPlayer.ChannelInstruments[channel])
					{
						case 0x40:
							instrument = Midi.Midi.Patches.Flute;
							offset = 12;
							break;

						case 0x41:
							instrument = Midi.Midi.Patches.Strings;
							offset = 12;
							break;

						case 0x42:
							instrument = Midi.Midi.Patches.Trombone;
							offset = 0;
							break;

						case 0x44:
							//instrument = Midi.Midi.Patches.PizzicatoStrings;
							instrument = Midi.Midi.Patches.Harp;
							offset = 12;
							break;

						case 0x43:
							drums = Midi.Midi.Drums.SnareDrum;
							break;

						case 0x45:
							instrument = Midi.Midi.Patches.Timpani;
							offset = -24;
							break;

						case 0x46:
							drums = Midi.Midi.Drums.CrashCymbal;
							break;

						default:
							instrument = Midi.Midi.Patches.SquareLead;
							break;
					}
					break;

				case 55:
					switch (SongPlayer.ChannelInstruments[channel])
					{
						case 0x40:
							//instrument = Midi.Midi.Patches.Trumpet;
							instrument = Midi.Midi.Patches.BrassSection;
							break;

						case 0x41:
							instrument = Midi.Midi.Patches.Strings;
							offset = 12;
							break;

						case 0x42:
							drums = Midi.Midi.Drums.SnareDrum;
							break;

						case 0x43:
							drums = Midi.Midi.Drums.CrashCymbal2;
							break;

						case 0x44:
							instrument = Midi.Midi.Patches.Timpani;
							offset = -24;
							break;

						case 0x45:
							instrument = Midi.Midi.Patches.Flute;
							offset = 12;
							break;

						default:
							instrument = Midi.Midi.Patches.SquareLead;
							break;
					}
					break;

				default:
					instrument = Midi.Midi.Patches.GrandPiano;
					break;
			}

			if(drums == -1)
				Midi.Midi.ProgramChange(channel, instrument);

			ChannelDrums[channel] = drums;
			ChannelOffset[channel] = offset;
		}
	}
}