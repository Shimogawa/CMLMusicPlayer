using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CMLMusicPlayer
{
	public class MusicPlayHandler
	{
		private bool musicEnd;
		private bool isStopped;
		private int currentSong;
		private Thread playSongThread;
		private IWavePlayer outputDevice;
		private IWaveProvider waveProvider;

		public event EventHandler OnMusicEnd;

		public string PlaySrc { get; set; }
		public Version Version { get; }
		public List<string> SongName { get; private set; }

		private MusicPlayHandler()
		{
			Version = typeof(Program).Assembly.GetName().Version;
			musicEnd = true;
			isStopped = false;
			currentSong = -1;
			outputDevice = new WaveOutEvent
			{
				Volume = 0.7f
			};
		}

		public MusicPlayHandler(string src) : this()
		{
			PlaySrc = src;
			SongName = new List<string>(Directory.EnumerateFiles(src));
			waveProvider = new AudioFileReader(SongName[0]);
		}

		///// <summary>
		///// Call this function every frame.
		///// </summary>
		//public void Update()
		//{
		//	while (true)
		//	{
		//		if (musicEnd)
		//		{
		//			NextSong();
		//			return;
		//		}
		//	}
		//}

		public void Run()
		{
			currentSong = 0;
			Play();
		}

		private void PlaySong()
		{
			outputDevice.Stop();
			waveProvider = new AudioFileReader(SongName[currentSong]);
			outputDevice.Init(waveProvider);
			outputDevice.Play();
			while (outputDevice.PlaybackState == PlaybackState.Playing)
			{
				Thread.Sleep(1000);
			}
			musicEnd = true;
			// 事件驱动
			OnMusicEnd(this, new EventArgs());
		}

		private void NextSong()
		{
			currentSong = (currentSong + 1) % SongName.Count;
			playSongThread = new Thread(PlaySong);
			playSongThread.Start();
			musicEnd = false;
		}

		private void PrevSong()
		{
			currentSong--;
			if (currentSong < 0)
				currentSong += SongName.Count;
			playSongThread = new Thread(PlaySong);
			playSongThread.Start();
			musicEnd = false;
		}

		/// <summary>
		/// Interrupt and play the next song.
		/// </summary>
		public void SwitchNextSong()
		{
			playSongThread.Abort();
			musicEnd = true;
		}

		/// <summary>
		/// Interrupt and play the previous song.
		/// </summary>
		public void SwitchPrevSong()
		{
			playSongThread.Abort();
			PrevSong();
		}

		/// <summary>
		/// Stop current song entirely.
		/// </summary>
		public void Stop()
		{
			isStopped = true;
			playSongThread.Abort();
			playSongThread = null;
		}

		/// <summary>
		/// Start playing the song from the beginning.
		/// </summary>
		public void Play()
		{
			isStopped = false;
			musicEnd = false;
			playSongThread = new Thread(PlaySong);
			playSongThread.Start();
		}

		public void CleanUp()
		{
			isStopped = true;
			outputDevice.Dispose();
			if (playSongThread != null)
				playSongThread.Abort();
			playSongThread = null;
		}
	}
}