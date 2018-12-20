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

		private bool musicEnd = true;
		
		private int currentSong = 0;

		private Thread playSongThread;



		public string PlaySrc { get; set; }

		public Version Version { get; set; }

		public List<string> Files { get; private set; }

		public MusicPlayer() { }

		public MusicPlayer(string src) : this()
		{
			PlaySrc = src;
			Files = new List<string>(Directory.EnumerateFiles(src));
		}

		public void Run()
		{
			DrawCredits();
			currentSong = 0;

			// TODO: put key press event into a new thread/event.
//			while (timer.Enabled)
//			{
//				var key = Console.ReadKey(true).Key;
//				switch (key)
//				{
//					case ConsoleKey.Q:
//						{
//							timer.Enabled = false;
//							Exit();
//							break;
//						}
//					case ConsoleKey.N:
//						NextSong();
//						break;
//					default:
//						break;
//				}
//			}
		}

		private void CheckKeyboard()
		{
			// dummy for now
		}

		private void Task(object source, ElapsedEventArgs e)
		{
//			frames++;
			//DrawProgress();

			if (musicEnd && (playSongThread == null || !playSongThread.IsAlive))
			{
				DrawList();
				playSongThread = new Thread(PlaySong);
				playSongThread.Start();
				musicEnd = false;
			}

		}

		public void DrawList()
		{
			for (int i = 0; i < Files.Count; i++)
			{
				Console.WriteLine($"{(i == currentSong ? ">" : "")}{Files[i]}");
			}
		}

		private void DrawProgress()
		{
			int height = Console.WindowHeight;
			if (Thread.CurrentThread.CurrentUICulture.Equals(CultureInfo.GetCultureInfo("zh-CN")))
				height--;
			Console.SetCursorPosition(0, height - 1);

//			Console.Write("Draw Prog, Avg FR: {0:F2}, Cur FR: {1:F2}\r",
//				FrameRate1(), FrameRate2());
		}

		private void PlaySong()
		{
			using (var audioFile = new AudioFileReader(Files[currentSong]))
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
			playSongThread.Abort();
			playSongThread = null;
			currentSong++;
			if (currentSong >= Files.Count) currentSong = currentSong - Files.Count;
			musicEnd = true;
		}

		private void DrawCredits()
		{
			Console.Clear();
			Console.WriteLine(Strings.Credits, Version.ToString());
			Thread.Sleep(2000);
		}

		private void Exit()
		{
			Console.Clear();
			Console.WriteLine(Strings.ExitWords);
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
