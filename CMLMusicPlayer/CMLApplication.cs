using CMLMusicPlayer.Arguments;
using CMLMusicPlayer.UI;
using System;
using System.Text;

namespace CMLMusicPlayer
{
	public class CMLApplication
	{
		public Random Random
		{
			get { return random; }
		}

		public static CMLApplication Me;

		private long currentTime;
		private double deltaTime;
		private Random random;
		private int count;
		private Renderer renderer;

		private readonly int frameRate;

		private const int DEFAULT_FRAMERATE = 20;

		public CMLApplication(CMLConfig config)
		{
			this.frameRate = DEFAULT_FRAMERATE;
			init();
		}

		private void init()
		{
			Console.CursorVisible = false;
			count = 0;
			random = new Random();
			renderer = new Renderer(10, 10);
			Me = this;
			Console.OutputEncoding = Encoding.UTF8;
		}
		
		private void draw()
		{
			renderer.ResetBuffer();
			renderer.Test(0, 0);
			renderer.Present();
		}

		public void Run()
		{
			currentTime = DateTime.Now.Ticks;
			while (true)
			{
				// 主要绘制函数区域

				draw();


				long delta;

				// [1s / frameRate] refresh duration 
				while ((delta = DateTime.Now.Ticks - currentTime) < (1e7 / frameRate))
					;
				deltaTime = delta / 1e7;
				currentTime = DateTime.Now.Ticks;
			}
		}
	}
}
