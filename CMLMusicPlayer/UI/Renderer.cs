using CMLMusicPlayer.Utils;
using System;

namespace CMLMusicPlayer.UI
{
	//public enum CMLCharacter
	//{
	//	EMPTY = -1,
	//	FULL_BLOCK = 1000000 + 2588,
	//}

	public class Renderer
	{
		public int XLimit
		{
			get;
		}

		public int YLimit
		{
			get;
		}

		private readonly int bufferLimitX;
		private readonly int bufferLimitY;

		private const int DOUBLE_CHAR = 0x100;
		private CoordMapper coordMapper;

		public Renderer(int maxX, int maxY)
		{
			XLimit = maxX;
			YLimit = maxY;
			bufferLimitX = maxX * 2;
			bufferLimitY = maxY;
			ConsoleUtil.InitializeBuffer(maxX, maxY);
		}


		public void Draw(Shape shape, int x, int y)
		{
			shape.SetPos(x, y);
			shape.Draw(this);
		}

		public void Present()
		{
			ConsoleUtil.Present();
			ConsoleUtil.SwapBuffer();
		}

		public void ResetBuffer()
		{
			ConsoleUtil.ClearBuffer();
		}

		public void SetChar(int x, int y, char ch)
		{
			if (x >= XLimit || y >= YLimit || x < 0 || y < 0)
			{
				throw new ArgumentOutOfRangeException("坐标超出界限");
			}
			ConsoleUtil.SetChar(y, x, ch);
			//var coord = coordMapper.QueryCoord(x, y);
			//gameScreen[coord.X, coord.Y] = ch;
			//if (ch >= DOUBLE_CHAR)
			//{
			//	coordMapper.Set(x, y);
			//	coord.X++;
			//	gameScreen[coord.X, coord.Y] = ch;
			//	invalidPoints[coord.X, coord.Y] = true;

			//}
			//else
			//{
			//	coordMapper.Clear(x, y);
			//}

		}

		public void DrawString(int x, int y, string str)
		{
			for(int i = 0; i < str.Length; i++)
			{
				SetChar(x + i, y, str[i]);
			}
			// 换行情况需要单独考虑
		}

		public void DrawStringMultiLine(int x, int y, string str, int rowLength)
		{
			throw new NotImplementedException();
		}

		//public void SetColumn(int col, string str)
		//{
		//	if (str.Length > XLimit)
		//		throw new NotImplementedException();

		//	// TODO: 如果穿过中文字符的后一格，需要将该中文字符也一并清除。

		//	for (int r = 0; r < str.Length; r++)
		//	{
		//		gameScreen[r, col] = str[r];
		//	}
		//}

		#region TEST
		public void Test(int r, int c)
		{
			for(int i = r; i < r + 5; i++)
			{
				for (int j = c; j < c + 5; j++)
				{
					SetChar(i, j, '█');
				}
			}
		}
		#endregion
	}
}
