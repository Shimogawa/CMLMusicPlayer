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
	// 这个类耦合过高了
	public class MusicPlayer
	{

		//private const long Jan1st1970 = 621355968000000000L;

		private readonly Timer timer = new Timer(1000d);

		private bool musicEnd = true;

		private int frames = 0;
		private DateTime startOn;
		private DateTime lastFrame;

		// START song files
		#region song files
		private List<string> files;
		private int current = 0;
		#endregion
		// END song files

		private Thread playSongThread;



		public string PlaySrc { get; set; }

		public Version Version { get; set; }

		public int FR { get; set; }

		public MusicPlayer() { }

		public MusicPlayer(string src) : this()
		{
			PlaySrc = src;
			files = new List<string>(Directory.EnumerateFiles(src));
		}

		public void Run()
		{
			DrawCredits();
			current = 0;

			timer.Interval = 1000.0 / FR;
			timer.Elapsed += Task;
			timer.AutoReset = true;

			startOn = DateTime.UtcNow;
			lastFrame = DateTime.UtcNow;
			timer.Enabled = true;

			// TODO: put key press event into a new thread/event.
			while (timer.Enabled)
			{
				var key = Console.ReadKey(true).Key;
				switch (key)
				{
					case ConsoleKey.Q:
						{
							timer.Enabled = false;
							Exit();
							break;
						}
					case ConsoleKey.N:
						NextSong();
						break;
					default:
						break;
				}
			}
		}

		private void CheckKeyboard()
		{
			// dummy for now
		}

		private void Task(object source, ElapsedEventArgs e)
		{
			frames++;
			//DrawProgress();

			if (musicEnd && (playSongThread == null || !playSongThread.IsAlive))
			{
				DrawList();
				playSongThread = new Thread(PlaySong);
				playSongThread.Start();
				musicEnd = false;
			}

		}

		private void DrawList()
		{
			Console.Clear();
			for (int i = 0; i < files.Count; i++)
			{
				Console.WriteLine($"{(i == current ? ">" : "")}{files[i]}");
			}
		}

		private void DrawProgress()
		{
			int height = Console.WindowHeight;
			if (Thread.CurrentThread.CurrentUICulture.Equals(CultureInfo.GetCultureInfo("zh-CN")))
				height--;
			Console.SetCursorPosition(0, height - 1);

			Console.Write("Draw Prog, Avg FR: {0:F2}, Cur FR: {1:F2}\r",
				FrameRate1(), FrameRate2());
		}

		private void PlaySong()
		{
			using (var audioFile = new AudioFileReader(files[current]))
			using (var outputDevice = new WaveOutEvent() { Volume = 0.7f })
			{
				outputDevice.Init(audioFile);
				outputDevice.Play();
				while (outputDevice.PlaybackState == PlaybackState.Playing && timer.Enabled)
				{
					Thread.Sleep(1000);
				}
			}
			musicEnd = true;
		}

		private void NextSong()
		{
			playSongThread.Abort();
			current++;
			if (current >= files.Count) current = current - files.Count;
			musicEnd = true;
		}

		private void DrawCredits()
		{
			Console.Clear();
			Console.WriteLine(Strings.Credits, Version.ToString());
			Thread.Sleep(2000);
		}

		private double FrameRate1()
		{
			var r = (DateTime.UtcNow - startOn).TotalMilliseconds;

			return 1000.0 / (r / frames);
		}

		private double FrameRate2()
		{
			var r = (DateTime.UtcNow - lastFrame).TotalMilliseconds;
			lastFrame = DateTime.UtcNow;
			return 1000.0 / r;
		}

		private void Exit()
		{
			Console.Clear();
			Console.WriteLine(Strings.ExitWords);
		}

		public void Test()
		{
			foreach (var file in files)
			{
				using (var audioFile = new AudioFileReader(file))
				using (var outputDevice = new WaveOutEvent() { Volume = 10 })
				{
					outputDevice.Init(audioFile);
					outputDevice.Play();
					while (outputDevice.PlaybackState == PlaybackState.Playing)
					{
						Thread.Sleep(1000);
					}
				}
			}
		}
	}
}
