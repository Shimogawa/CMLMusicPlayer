using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMLMusicPlayer.UI
{
	// 原型，等待填充
	public abstract class Shape
	{
		private Coordinate<int> position;

		public abstract void Draw(Renderer renderer);

		public abstract void SetPos(int x, int y);
	}
}
