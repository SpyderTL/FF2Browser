using System;
using System.IO;

namespace SuperNintendo
{
	internal class RomFile
	{
		internal static byte[] Data;

		internal static void Load(string path)
		{
			Rom.Data = File.ReadAllBytes(path);

			ConsoleRom.Load();
		}
	}
}