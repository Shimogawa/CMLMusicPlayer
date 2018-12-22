﻿using CMLMusicPlayer.Arguments;
using CMLMusicPlayer.UI;
using System;
using System.Text;
using System.Threading;
using CMLMusicPlayer.Resources;
using CMLMusicPlayer.Utils;

namespace CMLMusicPlayer
{
	public class CMLApplication
	{
		public Random Random { get; private set; }

		/// <summary>
		/// If the application is running.
		/// </summary>
		public bool IsEnabled
		{
			get; private set;
		}
		
		/// <summary>
		/// The music player.
		/// </summary>
		public MusicPlayer MusicPlayer { get; private set; }

		/// <summary>
		/// The instance of this app.
		/// </summary>
		public static CMLApplication Me;

		private long currentTime;
		private double deltaTime;
		private int count;
		private Renderer renderer;

		private Thread keyboardThread;

		private readonly int frameRate;
		private readonly string musicFolder;
		private readonly long ticksPerFrame;

		public CMLApplication(CMLConfig config)
		{
			this.frameRate = config.FrameRate;
			this.musicFolder = config.MusicFolder;
			ticksPerFrame = (long)(1e7 / frameRate);
			init();
		}

		private void init()
		{
			count = 0;
			Random = new Random();
			renderer = new Renderer(50, 25);
			MusicPlayer = new MusicPlayer(musicFolder);
			keyboardThread = new Thread(keyboardControl);
			Me = this;
			IsEnabled = true;

			// Console Settings
			Console.OutputEncoding = Encoding.UTF8;
			Console.CursorVisible = false;
			ConsoleUtil.FixConsoleWindowSize();
		}
		
		private void draw()
		{
			renderer.ResetBuffer();
			renderer.Test(0, 0);
			renderer.DrawString(0, 5, "你好");
			//renderer.SetLine(6, "你好");
			renderer.Present();
			Console.WriteLine(1 / deltaTime);   // Frame rate
		}

		public void Run()
		{
			Console.Clear();
			keyboardThread.Start();
			currentTime = DateTime.Now.Ticks;
			while (IsEnabled)
			{
				// 更新区域

				draw();
				MusicPlayer.Update();
				
				// 更新区域

				long delta;

				// ticksPerFrame refresh duration 
				while ((delta = DateTime.Now.Ticks - currentTime) < ticksPerFrame)
					;
				deltaTime = delta / 1e7;
				currentTime = DateTime.Now.Ticks;
			}

			exit();
			
			// 处理Dispose
			MusicPlayer.Dispose();
		}

		private void keyboardControl()
		{
			while (IsEnabled)
			{
				var key = Console.ReadKey(true).Key;
				switch (key)
				{
					case ConsoleKey.Q:
						{
							MusicPlayer.Stop();
							IsEnabled = false;
							break;
						}
					case ConsoleKey.N:
						{
							MusicPlayer.SwitchNextSong();
							break;
						}
					case ConsoleKey.M:
						{
							MusicPlayer.SwitchPrevSong();
							break;
						}
					case ConsoleKey.S:
						{
							MusicPlayer.Stop();
							break;
						}
					case ConsoleKey.A:
						{
							MusicPlayer.Play();
							break;
						}
					default:
						break;
				}
			}
		}

		private void exit()
		{
			Console.Clear();
			Console.WriteLine(Strings.ExitWords);
			Console.ReadLine();
		}
	}
}
