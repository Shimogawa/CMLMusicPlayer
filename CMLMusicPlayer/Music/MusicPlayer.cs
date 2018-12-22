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
	public class MusicPlayer : IDisposable
	{

		private bool musicEnd;
		private bool isStopped;
		private int currentSong;

		private Thread playSongThread;

		public string PlaySrc { get; set; }

		public Version Version { get; }

		public List<string> SongName { get; private set; }

		private MusicPlayer()
		{
			Version = typeof(Program).Assembly.GetName().Version;
			musicEnd = true;
			isStopped = false;
			currentSong = -1;
		}

		public MusicPlayer(string src) : this()
		{
			PlaySrc = src;
			SongName = new List<string>(Directory.EnumerateFiles(src));
		}

		/// <summary>
		/// Call this function every frame.
		/// </summary>
		public void Update()
		{
			if (isStopped)
				return;
			if (musicEnd)
			{
				NextSong();
				return;
			}
		}

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

		private void NextSong()
		{
			currentSong++;
			if (currentSong >= SongName.Count)
				currentSong -= SongName.Count;
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

		public void Dispose()
		{
			if (playSongThread != null)
				playSongThread.Abort();
			playSongThread = null;
		}
	}
}
