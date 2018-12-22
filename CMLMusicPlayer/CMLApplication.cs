using CMLMusicPlayer.Arguments;
using CMLMusicPlayer.UI;
using System;
using System.Text;
using System.Threading;
using CMLMusicPlayer.Resources;

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

		private readonly int frameRate;
		private readonly string musicFolder;
		private readonly long ticksPerFrame;

		public CMLApplication(CMLConfig config)
		{
			frameRate = config.FrameRate;
			musicFolder = config.MusicFolder;
			ticksPerFrame = (long)(1e7 / frameRate);
			init();
		}

		private void init()
		{
			Random = new Random();
			renderer = new Renderer(50, 25);
			PlayHandler = new MusicPlayHandler(musicFolder);
			Me = this;
			IsEnabled = true;
			Console.OutputEncoding = Encoding.UTF8;
			Console.CursorVisible = false;

			keyBoardHandler = new KeyBoardHandler();
			// 可以非硬编码，由配置文件决定
			keyBoardHandler.Register(ConsoleKey.Q, () => { Exit(); });
			keyBoardHandler.Register(ConsoleKey.N, () => { PlayHandler.SwitchNextSong(); });
			keyBoardHandler.Register(ConsoleKey.M, () => { PlayHandler.SwitchPrevSong(); });
			keyBoardHandler.Register(ConsoleKey.S, () => { PlayHandler.Stop(); });
			keyBoardHandler.Register(ConsoleKey.A, () => { PlayHandler.Play(); });
		}
		
		private void drawLoop()
		{
			Console.Clear();
			currentTime = DateTime.Now.Ticks;
			while (IsEnabled)
			{
				// 更新区域
				// draw();

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
			drawLoop();
			// 处理Dispose
			Exit();
		}

		private void PlayHandler_OnMusicEnd(object sender, EventArgs e)
		{
			// sender可以是任意对象 （按需求来
			var playhandler = sender as MusicPlayHandler;
			playhandler.SwitchNextSong();
		}

		public void Exit()
		{
			PlayHandler.CleanUp();
			IsEnabled = false;
			Console.Clear();
			Console.WriteLine(Strings.ExitWords);
			Console.ReadLine();
		}
	}
}
