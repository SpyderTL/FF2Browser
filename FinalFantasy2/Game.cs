using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalFantasy2
{
	internal static class Game
	{
		internal static readonly int SongAddressTable = 0x06F21D;
		internal static readonly int SongCount = 56;

		internal static readonly int[] SongBlockAddresses = new int[] { 0x04C000, 0x058000, 0x068000, 0x078000, 0x088000 };
		internal static readonly int[] SongAddressBlocks = new int[] { 0x000000, 0x004000, 0x00C000, 0x014000, 0x01C000 };

		internal const int FirstNote = 0x00;
		internal const int LastNote = 0xB3;
		internal const int FirstRest = 0xB4;
		internal const int LastRest = 0xC2;
		internal const int FirstTie = 0xC3;
		internal const int LastTie = 0xD1;

		internal static readonly int[] NoteDurations = new int[] { 192, 144, 96, 72, 64, 48, 36, 32, 24, 16, 12, 8, 6, 4, 3 };

		internal static readonly string[] SongNames = new string[]
		{
			"Empty",
			"Prologue",
			"Song 3",
			"Song 4",
			"Song 5",
			"Song 6",
			"Song 7",
			"Final Boss",
			"Victory",

			"Song 10",
			"Song 11",
			"Dreadful Fight",
			"Song 13",
			"Ship",
			"Airship",
			"Song 16",
			"Good Night",
			"Song 18",
			"Song 19",

			"Rosa's Theme",
			"Song 21",
			"Title",
			"Song 23",
			"Song 24",
			"Song 25",
			"Song 26",
			"Battle Theme 2",
			"Song 28",
			"Song 29",

			"Song 30",
			"Song 31",
			"Song 32",
			"Song 33",
			"Song 34",
			"Dancing Calbrena",
			"Song 36",
			"Song 37",
			"Song 38",
			"Dancing Ladies",

			"Battle Theme",
			"Song 41",
			"Song 42",
			"Song 43",
			"Song 44",
			"Red Wings",
			"Song 46",
			"Song 47",
			"Song 48",
			"Song 49",

			"Song 50",
			"Song 51",
			"Song 52",
			"Song 53",
			"Ending Part 1",
			"Ending Part 2",
			"Ending Part 3",
		};
	}
}
