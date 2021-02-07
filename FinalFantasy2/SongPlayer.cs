using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace FinalFantasy2
{
	internal static class SongPlayer
	{
		internal static int[] ChannelNotes;
		internal static int[] ChannelOctaves;
		internal static int[] ChannelVolumes;
		internal static bool[] ChannelTriggers;
		internal static long[] ChannelTimers;
		internal static int[] ChannelInstruments;
		internal static int[] ChannelDurations;
		internal static long LastUpdate;
		internal static Stack<int>[] ChannelLoops;
		internal static Stack<int>[] ChannelRepeats;
		internal static Stack<int>[] ChannelCounters;
		internal static int Tempo;

		internal static void Reset()
		{
			ChannelInstruments = new int[8];
			ChannelNotes = Enumerable.Repeat(-1, 8).ToArray();
			ChannelOctaves = new int[8];
			ChannelVolumes = new int[8];
			ChannelDurations = new int[8];
			ChannelTimers = new long[8];
			ChannelTriggers = new bool[8];
			ChannelLoops = Enumerable.Range(0, 8).Select(x => new Stack<int>()).ToArray();
			ChannelRepeats = Enumerable.Range(0, 8).Select(x => new Stack<int>()).ToArray();
			ChannelCounters = Enumerable.Range(0, 8).Select(x => new Stack<int>()).ToArray();
			Tempo = 10;
		}

		internal static void Play()
		{
			LastUpdate = Environment.TickCount64;
		}

		internal static void Update()
		{
			var now = Environment.TickCount64;
			var elapsed = now - LastUpdate;
			LastUpdate = now;

			for (var channel = 0; channel < ChannelTimers.Length; channel++)
			{
				if (SongReader.ChannelPositions[channel] == 0)
					continue;

				ChannelTriggers[channel] = false;
				ChannelTimers[channel] -= elapsed * Tempo;

				while (ChannelTimers[channel] <= 0 &&
					SongReader.ChannelPositions[channel] != 0)
				{
					ChannelReader.Position = SongReader.ChannelPositions[channel];
					ChannelReader.Read();
					SongReader.ChannelPositions[channel] = ChannelReader.Position;

					switch (ChannelReader.EventType)
					{
						case ChannelReader.EventTypes.Note:
							ChannelNotes[channel] = ChannelReader.Note;
							ChannelTriggers[channel] = true;
							ChannelTimers[channel] += Game.NoteDurations[ChannelReader.Duration] * 1200;
							break;

						case ChannelReader.EventTypes.Tie:
							ChannelTimers[channel] += Game.NoteDurations[ChannelReader.Duration] * 1200;
							break;

						case ChannelReader.EventTypes.Rest:
							ChannelNotes[channel] = -1;
							ChannelTimers[channel] += Game.NoteDurations[ChannelReader.Duration] * 1200;
							break;

						case ChannelReader.EventTypes.Octave:
							ChannelOctaves[channel] = ChannelReader.Octave;
							break;

						case ChannelReader.EventTypes.OctaveUp:
							ChannelOctaves[channel]++;
							break;

						case ChannelReader.EventTypes.OctaveDown:
							ChannelOctaves[channel]--;
							break;

						case ChannelReader.EventTypes.Instrument:
							ChannelInstruments[channel] = ChannelReader.Instrument;
							break;

						case ChannelReader.EventTypes.Tempo:
							Tempo = ChannelReader.Tempo;
							break;

						case ChannelReader.EventTypes.Volume:
							ChannelVolumes[channel] = ChannelReader.Volume;
							break;

						case ChannelReader.EventTypes.LoopStart:
							ChannelLoops[channel].Push(ChannelReader.Loop);
							ChannelRepeats[channel].Push(ChannelReader.Repeat);
							ChannelCounters[channel].Push(0);
							break;

						case ChannelReader.EventTypes.LoopEnd:
							if (ChannelRepeats[channel].Peek() == ChannelCounters[channel].Peek())
							{
								ChannelLoops[channel].Pop();
								ChannelRepeats[channel].Pop();
								ChannelCounters[channel].Pop();
							}
							else
							{
								SongReader.ChannelPositions[channel] = ChannelLoops[channel].Peek();
								ChannelCounters[channel].Push(ChannelCounters[channel].Pop() + 1);
							}
							break;

						case ChannelReader.EventTypes.LoopExit:
							if (ChannelCounters[channel].Peek() == ChannelReader.Loop - 1)
							{
								SongReader.ChannelPositions[channel] = ChannelReader.Jump;
								ChannelLoops[channel].Pop();
								ChannelRepeats[channel].Pop();
								ChannelCounters[channel].Pop();
							}
							break;

						case ChannelReader.EventTypes.Jump:
							SongReader.ChannelPositions[channel] = ChannelReader.Jump;
							break;

						case ChannelReader.EventTypes.Stop:
							ChannelNotes[channel] = -1;
							SongReader.ChannelPositions[channel] = 0;
							break;
					}
				}
			}
		}

		internal static void Stop()
		{

		}
	}
}