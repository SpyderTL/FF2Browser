using System;
using System.Threading;
using FinalFantasy2;
using Windows;

namespace FF2Browser
{
	internal static class SongWindow
	{
		internal static SongForm Form;

		internal static Timer Timer = new Timer(Timer_Elapsed, null, Timeout.Infinite, 10);

		internal static void Show()
		{
			Form = new SongForm();

			Form.Timer.Tick += Timer_Tick;
			Form.PlayButton.Click += PlayButton_Click;
			Form.StopButton.Click += StopButton_Click;
			Form.FormClosed += Form_FormClosed;

			Form.Show();
		}

		private static void Timer_Tick(object sender, EventArgs e)
		{
			//Form.TrackLabel.Text = SongReader.Position.ToString("X4");

			//Form.Channel1Label.Text = SongPlayer.ChannelNotes[0] == -1 ? string.Empty : SongPlayer.ChannelNotes[0].ToString("X2");
			//Form.Channel2Label.Text = SongPlayer.ChannelNotes[1] == -1 ? string.Empty : SongPlayer.ChannelNotes[1].ToString("X2");
			//Form.Channel3Label.Text = SongPlayer.ChannelNotes[2] == -1 ? string.Empty : SongPlayer.ChannelNotes[2].ToString("X2");
			//Form.Channel4Label.Text = SongPlayer.ChannelNotes[3] == -1 ? string.Empty : SongPlayer.ChannelNotes[3].ToString("X2");
			//Form.Channel5Label.Text = SongPlayer.ChannelNotes[4] == -1 ? string.Empty : SongPlayer.ChannelNotes[4].ToString("X2");
			//Form.Channel6Label.Text = SongPlayer.ChannelNotes[5] == -1 ? string.Empty : SongPlayer.ChannelNotes[5].ToString("X2");
			//Form.Channel7Label.Text = SongPlayer.ChannelNotes[6] == -1 ? string.Empty : SongPlayer.ChannelNotes[6].ToString("X2");
			//Form.Channel8Label.Text = SongPlayer.ChannelNotes[7] == -1 ? string.Empty : SongPlayer.ChannelNotes[7].ToString("X2");

			Form.Channel1Label.Text = SongPlayer.ChannelNotes[0] == -1 ? string.Empty : SongPlayer.ChannelInstruments[0].ToString("X2");
			Form.Channel2Label.Text = SongPlayer.ChannelNotes[1] == -1 ? string.Empty : SongPlayer.ChannelInstruments[1].ToString("X2");
			Form.Channel3Label.Text = SongPlayer.ChannelNotes[2] == -1 ? string.Empty : SongPlayer.ChannelInstruments[2].ToString("X2");
			Form.Channel4Label.Text = SongPlayer.ChannelNotes[3] == -1 ? string.Empty : SongPlayer.ChannelInstruments[3].ToString("X2");
			Form.Channel5Label.Text = SongPlayer.ChannelNotes[4] == -1 ? string.Empty : SongPlayer.ChannelInstruments[4].ToString("X2");
			Form.Channel6Label.Text = SongPlayer.ChannelNotes[5] == -1 ? string.Empty : SongPlayer.ChannelInstruments[5].ToString("X2");
			Form.Channel7Label.Text = SongPlayer.ChannelNotes[6] == -1 ? string.Empty : SongPlayer.ChannelInstruments[6].ToString("X2");
			Form.Channel8Label.Text = SongPlayer.ChannelNotes[7] == -1 ? string.Empty : SongPlayer.ChannelInstruments[7].ToString("X2");

			//Form.Channel1Label.Text = SongPlayer.ChannelNotess[0] == -1 ? string.Empty : RomInstruments.Instruments[SongPlayer.ChannelInstrumentss[0]].Value1.ToString("X2");
			//Form.Channel2Label.Text = SongPlayer.ChannelNotess[1] == -1 ? string.Empty : RomInstruments.Instruments[SongPlayer.ChannelInstrumentss[1]].Value1.ToString("X2");
			//Form.Channel3Label.Text = SongPlayer.ChannelNotess[2] == -1 ? string.Empty : RomInstruments.Instruments[SongPlayer.ChannelInstrumentss[2]].Value1.ToString("X2");
			//Form.Channel4Label.Text = SongPlayer.ChannelNotess[3] == -1 ? string.Empty : RomInstruments.Instruments[SongPlayer.ChannelInstrumentss[3]].Value1.ToString("X2");
			//Form.Channel5Label.Text = SongPlayer.ChannelNotess[4] == -1 ? string.Empty : RomInstruments.Instruments[SongPlayer.ChannelInstrumentss[4]].Value1.ToString("X2");
			//Form.Channel6Label.Text = SongPlayer.ChannelNotess[5] == -1 ? string.Empty : RomInstruments.Instruments[SongPlayer.ChannelInstrumentss[5]].Value1.ToString("X2");
			//Form.Channel7Label.Text = SongPlayer.ChannelNotess[6] == -1 ? string.Empty : RomInstruments.Instruments[SongPlayer.ChannelInstrumentss[6]].Value1.ToString("X2");
			//Form.Channel8Label.Text = SongPlayer.ChannelNotess[7] == -1 ? string.Empty : RomInstruments.Instruments[SongPlayer.ChannelInstrumentss[7]].Value1.ToString("X2");

			//Form.Channel1Label.Text = SongReader.ChannelPositions[0].ToString("X4");
			//Form.Channel2Label.Text = SongReader.ChannelPositions[1].ToString("X4");
			//Form.Channel3Label.Text = SongReader.ChannelPositions[2].ToString("X4");
			//Form.Channel4Label.Text = SongReader.ChannelPositions[3].ToString("X4");
			//Form.Channel5Label.Text = SongReader.ChannelPositions[4].ToString("X4");
			//Form.Channel6Label.Text = SongReader.ChannelPositions[5].ToString("X4");
			//Form.Channel7Label.Text = SongReader.ChannelPositions[6].ToString("X4");
			//Form.Channel8Label.Text = SongReader.ChannelPositions[7].ToString("X4");

			Form.Channel1Label.Left = 80 + ((SongPlayer.ChannelNotes[0] + (SongPlayer.ChannelOctaves[0] * 12)) * 8);
			Form.Channel2Label.Left = 80 + ((SongPlayer.ChannelNotes[1] + (SongPlayer.ChannelOctaves[1] * 12)) * 8);
			Form.Channel3Label.Left = 80 + ((SongPlayer.ChannelNotes[2] + (SongPlayer.ChannelOctaves[2] * 12)) * 8);
			Form.Channel4Label.Left = 80 + ((SongPlayer.ChannelNotes[3] + (SongPlayer.ChannelOctaves[3] * 12)) * 8);
			Form.Channel5Label.Left = 80 + ((SongPlayer.ChannelNotes[4] + (SongPlayer.ChannelOctaves[4] * 12)) * 8);
			Form.Channel6Label.Left = 80 + ((SongPlayer.ChannelNotes[5] + (SongPlayer.ChannelOctaves[5] * 12)) * 8);
			Form.Channel7Label.Left = 80 + ((SongPlayer.ChannelNotes[6] + (SongPlayer.ChannelOctaves[6] * 12)) * 8);
			Form.Channel8Label.Left = 80 + ((SongPlayer.ChannelNotes[7] + (SongPlayer.ChannelOctaves[7] * 12)) * 8);
		}

		private static void PlayButton_Click(object sender, EventArgs e)
		{
			//Form.SongLabel.Text = SongReader.Position.ToString("X4");
			Form.Timer.Start();

			MidiPlayer.Start();
			SongPlayer.Play();

			Timer.Change(0, 10);
		}

		private static void StopButton_Click(object sender, EventArgs e)
		{
			Form.Timer.Stop();
			Timer.Change(Timeout.Infinite, 10);

			SongPlayer.Stop();
			MidiPlayer.Stop();
		}

		private static void Timer_Elapsed(object state)
		{
			Timer.Change(Timeout.Infinite, 10);

			SongPlayer.Update();
			MidiPlayer.Update();

			Timer.Change(10, 10);
		}

		private static void Form_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
		{
			Form.PlayButton.Click -= PlayButton_Click;
			Form.StopButton.Click -= StopButton_Click;
			Form.FormClosed -= Form_FormClosed;

			Form.Timer.Stop();
			Timer.Change(Timeout.Infinite, 10);

			SongPlayer.Stop();
			MidiPlayer.Stop();

			Form = null;
		}
	}
}