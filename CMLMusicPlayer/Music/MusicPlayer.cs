using System;
using System.Threading;
using NAudio.Wave;
using System.IO;
using System.Collections.Generic;

namespace CMLMusicPlayer
{
	public class MusicPlayer : IDisposable
	{

		private bool isMusicEnd;
		private bool isStopped;
		private bool isPaused;
		private int currentSong;

		private Thread playSongThread;
		private WaveOutEvent outputDevice;
		private AudioFileReader audioFile;

		public string PlaySrc { get; set; }

		public Version Version { get; }

		public List<string> SongName { get; private set; }

		private MusicPlayer()
		{
			Version = typeof(Program).Assembly.GetName().Version;
			isMusicEnd = true;
			isStopped = false;
			isPaused = false;
			currentSong = -1;
			outputDevice = null;
			audioFile = null;
		}

		public MusicPlayer(string src) : this()
		{
			if (!Directory.Exists(src) || Directory.GetFiles(src).Length == 0)
			{
				Console.WriteLine("No music file founded.");
				Console.ReadKey(true);
				Environment.Exit(0);
			}
			PlaySrc = src;
			SongName = new List<string>(Directory.EnumerateFiles(src));
		}

		/// <summary>
		/// Call this function every frame.
		/// </summary>
		public void Update()
		{
			if (isStopped || isPaused)
				return;
			if (isMusicEnd)
			{
				nextSong();
				return;
			}
		}

		private void PlaySong()
		{
			isMusicEnd = false;
			isStopped = false;
			isPaused = false;
			using (audioFile = new AudioFileReader(SongName[currentSong]))
			using (outputDevice = new WaveOutEvent())
			{
				outputDevice.Init(audioFile);
				outputDevice.Play();
				while (outputDevice.PlaybackState == PlaybackState.Playing ||
					outputDevice.PlaybackState == PlaybackState.Paused)
				{
					Thread.Sleep(1000);
				}
			}
			isMusicEnd = true;

			// 下面这个方法有问题，会引发异常，需要解决。
			// 目前先使用Thread解决播放问题
			//if (outputDevice == null)
			//{
			//	outputDevice = new WaveOutEvent();
			//	outputDevice.PlaybackStopped += OnPlaybackStopped;
			//}
			//if (audioFile == null)
			//{
			//	audioFile = new AudioFileReader(SongName[currentSong]);
			//	outputDevice.Init(audioFile);
			//}
			//outputDevice = new WaveOutEvent();
			//outputDevice.PlaybackStopped += OnPlaybackStopped;
			//audioFile = new AudioFileReader(SongName[currentSong]);
			//outputDevice.Init(audioFile);
			//outputDevice.Play();
			//musicEnd = false;
			//isStopped = false;
		}

		private void nextSong()
		{
			stopAll();
			currentSong++;
			if (currentSong >= SongName.Count)
				currentSong -= SongName.Count;
			playSongThread = new Thread(PlaySong);
			playSongThread.Start();
			isMusicEnd = false;
			isStopped = false;
			isPaused = false;
		}

		private void prevSong()
		{
			stopAll();
			currentSong--;
			if (currentSong < 0)
				currentSong += SongName.Count;
			playSongThread = new Thread(PlaySong);
			playSongThread.Start();
			isMusicEnd = false;
			isStopped = false;
			isPaused = false;
		}

		/// <summary>
		/// Interrupt and play the next song.
		/// </summary>
		public void SwitchNextSong()
		{
			nextSong();
		}

		/// <summary>
		/// Interrupt and play the previous song.
		/// </summary>
		public void SwitchPrevSong()
		{
			prevSong();
		}

		/// <summary>
		/// Stop current song entirely.
		/// </summary>
		public void Stop()
		{
			isStopped = true;
			stopAll();
		}

		/// <summary>
		/// Start playing the song from the beginning when stopped, and from where it is left when paused.
		/// </summary>
		public void Play()
		{
			if (isStopped || isMusicEnd)
			{
				playSongThread = new Thread(PlaySong);
				playSongThread.Start();
				return;
			}
			if (isPaused)
			{
				resume();
			}
		}

		/// <summary>
		/// Pause the audio.
		/// </summary>
		public void Pause()
		{
			isPaused = true;
			outputDevice.Pause();
		}

		/// <summary>
		/// Dispose everything.
		/// </summary>
		public void Dispose()
		{
			stopAll();
		}

		private void resume()
		{
			isPaused = false;
			outputDevice.Play();
		}

		//private void OnPlaybackStopped(object sender, StoppedEventArgs args)
		//{
		//	outputDevice.Stop();
		//	isMusicEnd = true;
		//}

		private void stopAll()
		{
			isStopped = true;
			outputDevice?.Stop();
			playSongThread?.Abort();
			playSongThread = null;
			outputDevice?.Dispose();
			outputDevice = null;
			audioFile?.Dispose();
			audioFile = null;
		}
	}
}
