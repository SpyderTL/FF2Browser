using System;
using System.Linq;
using System.Windows.Forms;
using FinalFantasy2;
using Windows;

namespace FF2Browser
{
	internal class ProgramWindow
	{
		internal static ExplorerForm Form;

		internal static void Show()
		{
			Application.SetHighDpiMode(HighDpiMode.SystemAware);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			SuperNintendo.Rom.Data = Properties.Resources.Rom;
			SuperNintendo.ConsoleRom.Load();

			Form = new ExplorerForm();

			Form.TreeView.BeforeExpand += TreeView_BeforeExpand;
			Form.TreeView.AfterSelect += TreeView_AfterSelect;
			Form.ListView.MouseDoubleClick += ListView_MouseDoubleClick;

			var romNode = Form.TreeView.Nodes.Add("Rom", "Final Fantasy II");

			var songsNode = romNode.Nodes.Add("Songs", "Songs");
			songsNode.Nodes.Add("Loading");

			Application.Run(Form);
		}

		private static void ListView_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (Form.TreeView.SelectedNode == null ||
				Form.TreeView.SelectedNode.Parent == null)
				return;

			var item = Form.ListView.SelectedItems.Cast<ListViewItem>().FirstOrDefault();

			if (item == null)
				return;

			var node = Form.TreeView.SelectedNode;

			if (node.Name == "Songs")
			{
				var song = int.Parse(item.Name);
				var address = int.Parse(item.SubItems[1].Text, System.Globalization.NumberStyles.HexNumber);

				var position = address;

				var length = SuperNintendo.Console.Memory[position++] | (SuperNintendo.Console.Memory[position++] << 8);

				//Array.Copy(SuperNintendo.Console.Memory, position, SuperNintendo.Apu.Memory, 0x2000, length);

				for (var index = 0; index < length; index++)
				{
					var source = ((position & 0xff0000) >> 1) | (position & 0x7fff);
					source += index;
					source = ((source & 0x7f8000) << 1) | (source & 0x7fff) | 0x8000;

					var destination = 0x2000 + index;

					SuperNintendo.Apu.Memory[destination] = SuperNintendo.Console.Memory[source];
				}

				SongReader.Position = 0x2000;
				SongReader.Read();

				MidiPlayer.Song = song;
				SongPlayer.Reset();

				SongWindow.Show();
			}
		}

		private static void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			Form.ListView.Clear();

			if (e.Node.Parent == null)
				return;

			if (e.Node.Name == "Songs")
			{
				Form.ListView.Clear();

				Form.ListView.View = View.Details;

				Form.ListView.Columns.Add("Name", 200);
				Form.ListView.Columns.Add("Address", 200);

				for (var song = 0; song < Game.SongCount; song++)
				{
					var address = Game.SongAddressTable + (song * 3);

					address = SuperNintendo.Console.Memory[address] + (SuperNintendo.Console.Memory[address + 1] << 8) + (SuperNintendo.Console.Memory[address + 2] << 16);

					var block = Enumerable.Range(0, Game.SongAddressBlocks.Length).Where(x => Game.SongAddressBlocks[x] < address).Last();

					address -= Game.SongAddressBlocks[block];
					address += Game.SongBlockAddresses[block];

					var item = Form.ListView.Items.Add(song.ToString(), Game.SongNames[song], 0);
					item.SubItems.Add(address.ToString("X4"));
				}
			}
		}

		private static void TreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
		{
			if (e.Node.Parent == null)
				return;

			switch (e.Node.Name)
			{
				case "Songs":
					e.Node.Nodes.Clear();

					for (var song = 0; song < Game.SongCount; song++)
					{
						var address = Game.SongAddressTable + (song * 3);

						address = SuperNintendo.Console.Memory[address] + (SuperNintendo.Console.Memory[address + 1] << 8) + (SuperNintendo.Console.Memory[address + 2] << 16);

						var block = Enumerable.Range(0, Game.SongAddressBlocks.Length).Where(x => Game.SongAddressBlocks[x] < address).Last();

						address -= Game.SongAddressBlocks[block];
						address += Game.SongBlockAddresses[block];

						var songNode = e.Node.Nodes.Add(song.ToString(), address.ToString("X6"));
						songNode.Nodes.Add("Loading...");
					}

					break;

				default:
					switch (e.Node.Parent.Name)
					{
						case "Songs":
							e.Node.Nodes.Clear();

							var song = int.Parse(e.Node.Name);

							var address = Game.SongAddressTable + (song * 3);

							address = SuperNintendo.Console.Memory[address] | (SuperNintendo.Console.Memory[address + 1] << 8) | (SuperNintendo.Console.Memory[address + 2] << 16);

							var block = Enumerable.Range(0, Game.SongAddressBlocks.Length).Where(x => Game.SongAddressBlocks[x] < address).Last();

							var position = address - Game.SongAddressBlocks[block] + Game.SongBlockAddresses[block];

							var length = SuperNintendo.Console.Memory[position++] | (SuperNintendo.Console.Memory[position++] << 8);

							for (var channel = 0; channel < 8; channel++)
							{
								address = SuperNintendo.Console.Memory[position++] | (SuperNintendo.Console.Memory[position++] << 8);

								var channelNode = e.Node.Nodes.Add(channel.ToString(), address.ToString("X6"));
								channelNode.Nodes.Add("Loading...");
							}

							break;

						default:
							switch (e.Node.Parent.Parent.Name)
							{
								case "Songs":
									e.Node.Nodes.Clear();

									song = int.Parse(e.Node.Parent.Name);
									var channel = int.Parse(e.Node.Name);

									address = Game.SongAddressTable + (song * 3);

									address = SuperNintendo.Console.Memory[address] | (SuperNintendo.Console.Memory[address + 1] << 8) | (SuperNintendo.Console.Memory[address + 2] << 16);

									block = Enumerable.Range(0, Game.SongAddressBlocks.Length).Where(x => Game.SongAddressBlocks[x] < address).Last();

									position = address - Game.SongAddressBlocks[block] + Game.SongBlockAddresses[block];

									length = SuperNintendo.Console.Memory[position++] | (SuperNintendo.Console.Memory[position++] << 8);

									//Array.Copy(SuperNintendo.Console.Memory, position, SuperNintendo.Apu.Memory, 0x2000, length);

									for (var index = 0; index < length; index++)
									{
										var source = ((position & 0xff0000) >> 1) | (position & 0x7fff);
										source += index;
										source = ((source & 0x7f8000) << 1) | (source & 0x7fff) | 0x8000;

										var destination = 0x2000 + index;

										SuperNintendo.Apu.Memory[destination] = SuperNintendo.Console.Memory[source];
									}

									position = 0x2000 + (channel * 2);

									ChannelReader.Position = SuperNintendo.Apu.Memory[position++] | (SuperNintendo.Apu.Memory[position++] << 8);

									var reading = true;

									while (reading)
									{
										position = ChannelReader.Position;

										ChannelReader.Read();

										switch (ChannelReader.EventType)
										{
											case ChannelReader.EventTypes.Note:
												e.Node.Nodes.Add(position.ToString("X4") + " " + ChannelReader.Value.ToString("X2") + " - Note: " + ChannelReader.Note.ToString("X2") + " Duration: " + Game.NoteDurations[ChannelReader.Duration]);
												break;

											case ChannelReader.EventTypes.Rest:
												e.Node.Nodes.Add(position.ToString("X4") + " " + ChannelReader.Value.ToString("X2") + " - Rest: " + Game.NoteDurations[ChannelReader.Duration]);
												break;

											case ChannelReader.EventTypes.Tie:
												e.Node.Nodes.Add(position.ToString("X4") + " " + ChannelReader.Value.ToString("X2") + " - Tie: " + Game.NoteDurations[ChannelReader.Duration]);
												break;

											case ChannelReader.EventTypes.Instrument:
												e.Node.Nodes.Add(position.ToString("X4") + " " + ChannelReader.Value.ToString("X2") + " - Instrument: " + ChannelReader.Instrument.ToString("X2"));
												break;

											case ChannelReader.EventTypes.LoopStart:
												e.Node.Nodes.Add(position.ToString("X4") + " " + ChannelReader.Value.ToString("X2") + " - Loop: " + ChannelReader.Repeat);
												break;

											case ChannelReader.EventTypes.MasterVolume:
												e.Node.Nodes.Add(position.ToString("X4") + " " + ChannelReader.Value.ToString("X2") + " - Master Volume: " + ChannelReader.Volume.ToString("X6"));
												break;

											case ChannelReader.EventTypes.Volume:
												e.Node.Nodes.Add(position.ToString("X4") + " " + ChannelReader.Value.ToString("X2") + " - Volume: " + ChannelReader.Volume);
												break;

											case ChannelReader.EventTypes.VolumeFade:
												e.Node.Nodes.Add(position.ToString("X4") + " " + ChannelReader.Value.ToString("X2") + " - VolumeFade: " + ChannelReader.Volume + " Fade: " + ChannelReader.Fade);
												break;

											case ChannelReader.EventTypes.Pan:
												e.Node.Nodes.Add(position.ToString("X4") + " " + ChannelReader.Value.ToString("X2") + " - Pan: " + ChannelReader.Pan);
												break;

											case ChannelReader.EventTypes.PanFade:
												e.Node.Nodes.Add(position.ToString("X4") + " " + ChannelReader.Value.ToString("X2") + " - PanFade: " + ChannelReader.Pan + " Fade: " + ChannelReader.Fade);
												break;

											case ChannelReader.EventTypes.Chorus:
												e.Node.Nodes.Add(position.ToString("X4") + " " + ChannelReader.Value.ToString("X2") + " - Chorus: " + ChannelReader.Chorus);
												break;

											case ChannelReader.EventTypes.ReverbEcho:
												e.Node.Nodes.Add(position.ToString("X4") + " " + ChannelReader.Value.ToString("X2") + " - Reverb: " + ChannelReader.Reverb + " Echo: " + ChannelReader.Echo);
												break;

											case ChannelReader.EventTypes.Stop:
												e.Node.Nodes.Add(position.ToString("X4") + " " + ChannelReader.Value.ToString("X2") + " - Stop");
												reading = false;
												break;

											case ChannelReader.EventTypes.Jump:
												e.Node.Nodes.Add(position.ToString("X4") + " " + ChannelReader.Value.ToString("X2") + " - Jump: " + ChannelReader.Jump.ToString("X4"));
												reading = false;
												break;

											default:
												e.Node.Nodes.Add(position.ToString("X4") + " " + ChannelReader.Value.ToString("X2") + " - " + ChannelReader.EventType);
												break;
										}
									}

									break;
							}
							break;
					}
					break;
			}
		}
	}
}