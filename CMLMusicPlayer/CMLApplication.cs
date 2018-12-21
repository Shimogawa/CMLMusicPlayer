﻿using CMLMusicPlayer.Arguments;
using CMLMusicPlayer.UI;
using System;
using System.Text;
using System.Threading;

namespace CMLMusicPlayer
{
	public class CMLApplication
	{
		public Random Random
		{
			get { return random; }
		}

		/// <summary>
		/// If the application is running.
		/// </summary>
		public bool IsEnabled
		{
			get; private set;
		}

		/// <summary>
		/// The instance of this app.
		/// </summary>
		public static CMLApplication Me;

		private long currentTime;
		private double deltaTime;
		private Random random;
		private int count;
		private Renderer renderer;
		private MusicPlayer musicPlayer;

		private Thread keyboardThread;

		private readonly int frameRate;
		private readonly long ticksPerFrame;

		public CMLApplication(CMLConfig config)
		{
			this.frameRate = config.FrameRate;
			ticksPerFrame = (long)(1e7 / frameRate);
			init();
		}

		private void init()
		{
			Console.CursorVisible = false;
			count = 0;
			random = new Random();
			renderer = new Renderer(10, 10);
			musicPlayer = new MusicPlayer();
			keyboardThread = new Thread(keyboardControl);
			Me = this;
			IsEnabled = true;
			Console.OutputEncoding = Encoding.UTF8;
		}
		
		private void draw()
		{
			renderer.ResetBuffer();
			renderer.Test(0, 0);
			renderer.SetLine(6, "你好");
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
				// 主要绘制函数区域

				draw();


				long delta;

				// ticksPerFrame refresh duration 
				while ((delta = DateTime.Now.Ticks - currentTime) < ticksPerFrame)
					;
				deltaTime = delta / 1e7;
				currentTime = DateTime.Now.Ticks;
			}
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
							exit();
							break;
						}
					case ConsoleKey.N:
						//NextSong();
						break;
					default:
						break;
				}
			}
		}

		private void exit()
		{
			IsEnabled = false;
		}
	}
}
