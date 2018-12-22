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
	public class MusicPlayer : IWavePlayer
	{
		public PlaybackState PlaybackState => throw new NotImplementedException();

		public float Volume { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public event EventHandler<StoppedEventArgs> PlaybackStopped;

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		public void Init(IWaveProvider waveProvider)
		{
			throw new NotImplementedException();
		}

		public void Pause()
		{
			throw new NotImplementedException();
		}

		public void Play()
		{
			throw new NotImplementedException();
		}

		public void Stop()
		{
			throw new NotImplementedException();
		}
	}
}
