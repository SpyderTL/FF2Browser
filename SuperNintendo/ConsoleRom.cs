using System;

namespace SuperNintendo
{
	internal class ConsoleRom
	{
		internal static void Load()
		{
			Console.Memory = new byte[0x1000000];

			for (var bank = 0x00; bank < 0x7E; bank++)
			{
				if (Rom.Data.Length > bank * 0x8000)
					Array.Copy(Rom.Data, bank * 0x8000, Console.Memory, (bank * 0x10000) + 0x8000, 0x8000);
			}

			for (var bank = 0xFE; bank < 0x100; bank++)
			{
				if (Rom.Data.Length > bank * 0x8000)
					Array.Copy(Rom.Data, bank * 0x8000, Console.Memory, (bank * 0x10000) + 0x8000, 0x8000);
			}

			Array.Copy(Console.Memory, 0, Console.Memory, 0x800000, 0x7E0000);
		}
	}
}