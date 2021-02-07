using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperNintendo;

namespace FinalFantasy2
{
	public static class SongReader
	{
		public static int Position;
		public static readonly int[] ChannelPositions = new int[8];

		public static void Read()
		{
			for(var channel = 0; channel < 8; channel++)
				ChannelPositions[channel] = Apu.Memory[Position++] | (Apu.Memory[Position++] << 8);
		}
	}
}
