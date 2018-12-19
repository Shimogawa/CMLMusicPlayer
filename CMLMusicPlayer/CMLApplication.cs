using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMLMusicPlayer
{
	public class CMLApplication
	{
		private int currentTime;
		private double deltaTime;
		private readonly int frameRate;

		private const int DEFAULT_FRAMERATE = 60;

		public CMLApplication()
		{
			this.frameRate = DEFAULT_FRAMERATE;
		}

		// Program用来处理程序参数，之后将所需参数传递到CMLApplication里面执行
		public CMLApplication(int frameRate)
		{
			this.frameRate = frameRate;
		}


		public void Run()
		{
			currentTime = DateTime.Now.Millisecond;
			while (true)
			{
				// 主要绘制函数区域



				int delta = DateTime.Now.Millisecond - currentTime;

				// [1s / frameRate] refresh duration 
				while (delta < (1000.0 / frameRate))
					;
				deltaTime = delta / 1000.0;
			}
		}
	}
}
