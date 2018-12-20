using CMLMusicPlayer.Arguments;
using System;

namespace CMLMusicPlayer
{
	public class CMLApplication
	{
		private long currentTime;
		private double deltaTime;
		private Random random;
		private int count;

		private readonly int frameRate;

		private const int DEFAULT_FRAMERATE = 60;

		public CMLApplication(CMLConfig config)
		{
			this.frameRate = DEFAULT_FRAMERATE;
			init();
		}

		private void init()
		{
			count = 0;
			random = new Random();
		}
		
		private void draw()
		{
			Console.Clear();
			int width = Console.WindowWidth;
			int height = Console.WindowHeight;
			for(int i = 0; i < 10; i++)
			{
				for(int j = 0; j < 10; j++)
				{
					Console.SetCursorPosition(i, j);
					Console.Write('■');
				}
			}

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
