﻿using CMLMusicPlayer.Arguments;
using CMLMusicPlayer.UI;
using System;
using System.Text;
using System.Threading;
using CMLMusicPlayer.Resources;
using CMLMusicPlayer.Utils;
using CMLMusicPlayer.Music;

namespace CMLMusicPlayer
{
	public class CMLApplication
	{
		public Random Random { get; private set; }

		/// <summary>
		/// If the application is running.
		/// </summary>
		public bool IsEnabled{get; private set;}
		
		/// <summary>
		/// The music player.
		/// </summary>
		public MusicPlayHandler PlayHandler { get; private set; }

		/// <summary>
		/// The instance of this app.
		/// </summary>
		public static CMLApplication Me;

		private long currentTime;
		private double deltaTime;
		private Renderer renderer;
		private KeyBoardHandler keyBoardHandler;
		private SpectrumUI spectrumUI;

		private readonly int frameRate;
		private readonly string musicFolder;
		private readonly long ticksPerFrame;
		private readonly Coordinate<int> consoleSize;

		public CMLApplication(CMLConfig config)
		{
			frameRate = config.FrameRate;
			musicFolder = config.MusicFolder;
			ticksPerFrame = (long)(1e7 / frameRate);
			// 根据配置文件
			consoleSize = new Coordinate<int>(50, 25);
			init();
		}

		private void init()
		{
			Random = new Random();
			renderer = new Renderer(consoleSize.X, consoleSize.Y);
			PlayHandler = new MusicPlayHandler(musicFolder);
			spectrumUI = new SpectrumUI(consoleSize.X, consoleSize.Y);
			Me = this;
			IsEnabled = true;

			PlayHandler.OnFFTCalculated += PlayHandler_OnFFTCalculated;

			// Console Settings
			Console.OutputEncoding = Encoding.UTF8;
			Console.CursorVisible = false;
			ConsoleUtil.FixConsoleWindowSize();

			// Keyboard
			keyBoardHandler = new KeyBoardHandler();
			// 可以非硬编码，由配置文件决定
			keyBoardHandler.Register(ConsoleKey.Q, () => { Exit(); });
			keyBoardHandler.Register(ConsoleKey.N, () => { PlayHandler.SwitchNextSong(); });
			keyBoardHandler.Register(ConsoleKey.M, () => { PlayHandler.SwitchPrevSong(); });
			keyBoardHandler.Register(ConsoleKey.S, () => { PlayHandler.Stop(); });
			keyBoardHandler.Register(ConsoleKey.A, () => { PlayHandler.Play(); });
			keyBoardHandler.Register(ConsoleKey.P, () => { PlayHandler.Pause(); });
		}

		private void drawLoop()
		{
			Console.Clear();
			currentTime = DateTime.Now.Ticks;
			while (IsEnabled)
			{
				// 更新区域
				// draw();
				renderer.ResetBuffer();
				spectrumUI.Draw(renderer);
				renderer.Present();

				long delta;
				// ticksPerFrame refresh duration 
				while ((delta = DateTime.Now.Ticks - currentTime) < ticksPerFrame)
					;
				deltaTime = delta / 1e7;
				currentTime = DateTime.Now.Ticks;
			}
		}

		public void Run()
		{
			keyBoardHandler.Start();
			PlayHandler.Run();
			// EventHandler 可以优化
			PlayHandler.OnMusicEnd += PlayHandler_OnMusicEnd;
			PlayHandler.OnFFTCalculated += PlayHandler_OnFFTCalculated;
			drawLoop();
			// 处理Dispose
			Exit();
		}

		private void PlayHandler_OnFFTCalculated(object sender, FftEventArgs e)
		{
			spectrumUI.Update(e.Result);
		}

		private void PlayHandler_OnMusicEnd(object sender, EventArgs e)
		{
			// sender可以是任意对象 （按需求来
			var playhandler = sender as MusicPlayHandler;
			playhandler.SwitchNextSong();
		}

		public void Exit()
		{
			PlayHandler.Dispose();
			IsEnabled = false;
			Console.Clear();
			Console.WriteLine(Strings.ExitWords);
			Console.ReadKey(true);
		}
	}
}
