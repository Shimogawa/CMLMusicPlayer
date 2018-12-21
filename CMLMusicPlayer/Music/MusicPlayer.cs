using System;
using System.Globalization;
using System.Threading;
using Timer = System.Timers.Timer;
using CMLMusicPlayer.Resources;
using NAudio.Wave;
using System.Timers;
using System.IO;
using System.Collections.Generic;

namespace CMLMusicPlayer
{
	public class MusicPlayer
	{

		private bool musicEnd;
		private int currentSong;

		private Thread playSongThread;

		public string PlaySrc { get; set; }

		public Version Version { get; }

		public List<string> SongName { get; private set; }

		private MusicPlayer()
		{
			Version = typeof(Program).Assembly.GetName().Version;
			musicEnd = true;
			currentSong = 0;
		}

		public MusicPlayer(string src) : this()
		{
			PlaySrc = src;
			SongName = new List<string>(Directory.EnumerateFiles(src));
		}

		public void Update()
		{
			if (musicEnd && (playSongThread == null || !playSongThread.IsAlive))
			{
				playSongThread = new Thread(PlaySong);
				playSongThread.Start();
				musicEnd = false;
			}
		}

//		public void DrawList()
//		{
//			for (int i = 0; i < SongName.Count; i++)
//			{
//				Console.WriteLine($"{(i == currentSong ? ">" : "")}{SongName[i]}");
//			}
//		}

		private void PlaySong()
		{
			using (var audioFile = new AudioFileReader(SongName[currentSong]))
			using (var outputDevice = new WaveOutEvent() { Volume = 0.7f })
			{
				outputDevice.Init(audioFile);
				outputDevice.Play();
				while (outputDevice.PlaybackState == PlaybackState.Playing)
				{
					Thread.Sleep(1000);
				}
			}
			musicEnd = true;
		}

		/// <summary>
		/// Start playing next song.
		/// </summary>
		public void NextSong()
		{
			playSongThread.Abort();
			playSongThread = null;
			currentSong++;
			if (currentSong >= SongName.Count) currentSong = currentSong - SongName.Count;
			musicEnd = true;
		}

//		public void Test()
//		{
//			foreach (var file in Files)
//			{
//				using (var audioFile = new AudioFileReader(file))
//				using (var outputDevice = new WaveOutEvent() { Volume = 10 })
//				{
//					outputDevice.Init(audioFile);
//					outputDevice.Play();
//					while (outputDevice.PlaybackState == PlaybackState.Playing)
//					{
//						Thread.Sleep(1000);
//					}
//				}
//			}
//		}
	}
}
