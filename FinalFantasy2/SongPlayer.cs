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
		internal static int[] ChannelVolumeFadeTargets;
		internal static int[] ChannelVolumeFadeSpeeds;
		internal static long[] ChannelVolumeFadeTimers;
		internal static int[] ChannelPans;
		internal static int[] ChannelPanFadeTargets;
		internal static int[] ChannelPanFadeSpeeds;
		internal static long[] ChannelPanFadeTimers;
		internal static bool[] ChannelNoteTriggers;
		internal static long[] ChannelNoteTimers;
		internal static int[] ChannelInstruments;
		internal static int[] ChannelNoteDurations;
		internal static Stack<int>[] ChannelLoops;
		internal static Stack<int>[] ChannelRepeats;
		internal static Stack<int>[] ChannelCounters;
		internal static int Tempo;
		internal static int Chorus;
		internal static int Reverb;
		internal static int Echo;
		internal static long LastUpdate;

		internal static void Reset()
		{
			ChannelInstruments = new int[8];
			ChannelNotes = Enumerable.Repeat(-1, 8).ToArray();
			ChannelOctaves = new int[8];
			ChannelVolumes = new int[8];
			ChannelVolumeFadeTargets = new int[8];
			ChannelVolumeFadeSpeeds = new int[8];
			ChannelVolumeFadeTimers = new long[8];
			ChannelPans = new int[8];
			ChannelPanFadeTargets = new int[8];
			ChannelPanFadeSpeeds = new int[8];
			ChannelPanFadeTimers = new long[8];
			ChannelNoteDurations = new int[8];
			ChannelNoteTimers = new long[8];
			ChannelNoteTriggers = new bool[8];
			ChannelLoops = Enumerable.Range(0, 8).Select(x => new Stack<int>()).ToArray();
			ChannelRepeats = Enumerable.Range(0, 8).Select(x => new Stack<int>()).ToArray();
			ChannelCounters = Enumerable.Range(0, 8).Select(x => new Stack<int>()).ToArray();
			Tempo = 10;
			Chorus = 255;
			Reverb = 255;
			Echo = 0;
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

			for (var channel = 0; channel < ChannelNoteTimers.Length; channel++)
			{
				if (SongReader.ChannelPositions[channel] == 0)
					continue;

				if (ChannelVolumeFadeSpeeds[channel] != 0)
				{
					ChannelVolumeFadeTimers[channel] -= elapsed * 12;

					while (ChannelVolumeFadeTimers[channel] <= 0)
					{
						if (ChannelVolumes[channel] < ChannelVolumeFadeTargets[channel])
						{
							ChannelVolumes[channel]++;
							ChannelVolumeFadeTimers[channel] += ChannelVolumeFadeSpeeds[channel];
						}
						else if (ChannelVolumes[channel] > ChannelVolumeFadeTargets[channel])
						{
							ChannelVolumes[channel]--;
							ChannelVolumeFadeTimers[channel] += ChannelVolumeFadeSpeeds[channel];
						}
						else
						{
							ChannelVolumeFadeSpeeds[channel] = 0;
							break;
						}
					}
				}

				if (ChannelPanFadeSpeeds[channel] != 0)
				{
					ChannelPanFadeTimers[channel] -= elapsed * 12;

					while (ChannelPanFadeTimers[channel] <= 0)
					{
						if (ChannelPans[channel] < ChannelPanFadeTargets[channel])
						{
							ChannelPans[channel]++;
							ChannelPanFadeTimers[channel] += ChannelPanFadeSpeeds[channel];
						}
						else if (ChannelPans[channel] > ChannelPanFadeTargets[channel])
						{
							ChannelPans[channel]--;
							ChannelPanFadeTimers[channel] += ChannelPanFadeSpeeds[channel];
						}
						else
						{
							ChannelPanFadeSpeeds[channel] = 0;
							break;
						}
					}
				}

				ChannelNoteTriggers[channel] = false;
				ChannelNoteTimers[channel] -= elapsed * Tempo;

				while (ChannelNoteTimers[channel] <= 0 &&
					SongReader.ChannelPositions[channel] != 0)
				{
					ChannelReader.Position = SongReader.ChannelPositions[channel];
					ChannelReader.Read();
					SongReader.ChannelPositions[channel] = ChannelReader.Position;

					switch (ChannelReader.EventType)
					{
						case ChannelReader.EventTypes.Note:
							ChannelNotes[channel] = ChannelReader.Note;
							ChannelNoteTriggers[channel] = true;
							ChannelNoteTimers[channel] += Game.NoteDurations[ChannelReader.Duration] * 1200;
							break;

						case ChannelReader.EventTypes.Tie:
							ChannelNoteTimers[channel] += Game.NoteDurations[ChannelReader.Duration] * 1200;
							break;

						case ChannelReader.EventTypes.Rest:
							ChannelNotes[channel] = -1;
							ChannelNoteTimers[channel] += Game.NoteDurations[ChannelReader.Duration] * 1200;
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
							ChannelVolumeFadeSpeeds[channel] = 0;
							break;

						case ChannelReader.EventTypes.VolumeFade:
							ChannelVolumeFadeTargets[channel] = ChannelReader.Volume;
							ChannelVolumeFadeSpeeds[channel] = ChannelReader.Fade;
							ChannelVolumeFadeTimers[channel] = 0;
							break;

						case ChannelReader.EventTypes.Pan:
							ChannelPans[channel] = ChannelReader.Pan;
							ChannelPanFadeSpeeds[channel] = 0;
							break;

						case ChannelReader.EventTypes.PanFade:
							ChannelPanFadeTargets[channel] = ChannelReader.Pan;
							ChannelPanFadeSpeeds[channel] = ChannelReader.Fade;
							ChannelPanFadeTimers[channel] = 0;
							break;

						case ChannelReader.EventTypes.Chorus:
							Chorus = ChannelReader.Chorus;
							break;

						case ChannelReader.EventTypes.ReverbEcho:
							Reverb = ChannelReader.Reverb;
							Echo = ChannelReader.Echo;
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