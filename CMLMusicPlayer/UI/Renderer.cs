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

		private readonly char[,] gameScreen;
		private readonly bool[,] invalidPoints;
		private readonly int[] maxRenderWidths; 
		private const int DOUBLE_CHAR = 0x100;
		private CoordMapper coordMapper;

		public Renderer(int maxX, int maxY)
		{
			XLimit = maxX;
			YLimit = maxY;
			bufferLimitX = maxX * 2;
			bufferLimitY = maxY;
			gameScreen = new char[bufferLimitX, bufferLimitY];
			invalidPoints = new bool[bufferLimitX, bufferLimitY];
			coordMapper = new CoordMapper(bufferLimitX, bufferLimitY);
			maxRenderWidths = new int[bufferLimitX];
		}

		//private void drawSpecialChar(int c, int j)
		//{
		//	switch (c)
		//	{
		//		case (int)CMLCharacter.FULL_BLOCK:
		//			{
		//				if (j % 2 == 0)
		//					Console.Write('\u2588');
		//				break;
		//			}
		//		default:
		//			break;
		//	}
		//}

		public void Present()
		{
			for (int j = 0; j < bufferLimitY; j++)
			{
				for (int i = 0; i < maxRenderWidths[j]; i++)
				{
					if (!invalidPoints[i, j])
					{
						Console.SetCursorPosition(i, j);
						Console.Write(gameScreen[i, j]);
					}
				}
			}
		}

		public void ResetBuffer()
		{
			Console.SetCursorPosition(0, 0);
			for (int i = 0; i < bufferLimitX; i++)
			{
				for (int j = 0; j < bufferLimitY; j++)
				{
					gameScreen[i, j] = ' ';
					invalidPoints[i, j] = false;
				}
			}
			for(int j = 0; j < bufferLimitY; j++)
			{
				maxRenderWidths[j] = XLimit;
			}
		}

		public void SetChar(int x, int y, char ch)
		{
			var coord = coordMapper.QueryCoord(x, y);
			gameScreen[coord.X, coord.Y] = ch;
			if (ch >= DOUBLE_CHAR)
			{
				coordMapper.Set(x, y);
				coord.X++;
				gameScreen[coord.X, coord.Y] = ch;
				invalidPoints[coord.X, coord.Y] = true;

			}
			else
			{
				coordMapper.Clear(x, y);
			}
			if (coord.X > maxRenderWidths[y])
			{
				maxRenderWidths[y] = coord.X;
			}
		}

		//public void SetLine(int row, string str)
		//{
		//	if (str.Length > YLimit)
		//		throw new NotImplementedException();    // TODO

		//	for (int sPtr = 0, gPtr = 0; gPtr < YLimit && sPtr < str.Length; sPtr++, gPtr++)
		//	{
		//		gameScreen[row, gPtr] = str[sPtr];
		//		if (str[sPtr] >= 0x3000)
		//		{
		//			if (gPtr < YLimit - 1)
		//			{
		//				gPtr++;
		//				gameScreen[row, gPtr] = EMPTY_CHAR;
		//			}
		//		}
		//	}
		//}

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
					//用这个'█'就显示不了
					SetChar(i, j, '█');
				}
			}
		}
		#endregion
	}
}
