﻿using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace CMLMusicPlayer
{
	public class MusicPlayHandler
	{
		private bool isMusicEnd;
		private bool isStopped;
		private bool isPaused;
		private int currentSong;
		private Thread playSongThread;
		private IWavePlayer outputDevice;
		private AudioFileReader audioFile;		// 暂时使用AudioFileReader

		public event EventHandler OnMusicEnd;

		public string PlaySrc { get; set; }
		public Version Version { get; }
		public List<string> SongName { get; private set; }

		private MusicPlayHandler()
		{
			Version = typeof(Program).Assembly.GetName().Version;
			isMusicEnd = false;
			isStopped = true;
			isPaused = false;
			currentSong = 0;
			outputDevice = new WaveOutEvent
			{
				Volume = 0.7f
			};
		}

		public MusicPlayHandler(string src) : this()
		{
			PlaySrc = src;
			SongName = new List<string>(Directory.EnumerateFiles(src));
			audioFile = new AudioFileReader(SongName[0]);
		}

		public void Run()
		{
			// 前置操作


			Play();
		}

		private void playSong()
		{
			// 性能关键点，考虑用cache
			// Dispose 不要乱用， 容易引发异常且未观测到任何性能提升
			// audioFile.Dispose();
			audioFile = new AudioFileReader(SongName[currentSong]);
			outputDevice.Stop();
			outputDevice.Init(audioFile);
			outputDevice.Play();
			while (outputDevice.PlaybackState == PlaybackState.Playing ||
					outputDevice.PlaybackState == PlaybackState.Paused)
			{
				Thread.Sleep(1000);
			}
			isMusicEnd = true;
			// 事件驱动
			OnMusicEnd(this, new EventArgs());
		}

		private void playNew()
		{
			playSongThread = new Thread(playSong);
			playSongThread.Start();
			isMusicEnd = false;
			isStopped = false;
			isPaused = false;
		}

		private void resume()
		{
			isPaused = false;
			outputDevice.Play();
		}

		/// <summary>
		/// Interrupt and play the next song.
		/// </summary>
		public void SwitchNextSong()
		{
			if (!isMusicEnd)
			{
				playSongThread.Abort();
				isMusicEnd = true;
			}
			currentSong = (currentSong + 1) % SongName.Count;
			playNew();
		}

		/// <summary>
		/// Interrupt and play the previous song.
		/// </summary>
		public void SwitchPrevSong()
		{
			playSongThread.Abort();
			currentSong--;
			if (currentSong < 0)
				currentSong += SongName.Count;
			playNew();
		}

		/// <summary>
		/// Stop current song entirely.
		/// </summary>
		public void Stop()
		{
			if (isStopped)
				return;
			outputDevice.Stop();
			playSongThread.Abort();
			playSongThread = null;
			isStopped = true;
		}

		/// <summary>
		/// Start playing the song from the beginning.
		/// </summary>
		public void Play()
		{
			if (isStopped || isMusicEnd)
			{
				playNew();
				return;
			}
			if (isPaused)
			{
				resume();
			}
		}

		public void Pause()
		{
			isPaused = true;
			outputDevice.Pause();
		}

		public void Dispose()
		{
			outputDevice?.Dispose();
			audioFile?.Dispose();
			playSongThread?.Abort();
			playSongThread = null;
			outputDevice = null;
			audioFile = null;
			isStopped = true;
		}
	}
}