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
		public int Rows
		{
			get;
		}

		public int Cols
		{
			get;
		}

		private readonly int[,] gameScreen;

		public const int EMPTY_CHAR = -1;

		public Renderer(int rows, int cols)
		{
			Rows = rows;
			Cols = cols;
			gameScreen = new int[rows, cols];

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
			for(int i = 0; i < Rows; i++)
			{
				for(int j = 0; j < Cols; j++)
				{
					Console.SetCursorPosition(j, i);
					if (gameScreen[i, j] == EMPTY_CHAR)
					{
						continue;
					}
					Console.Write((char)gameScreen[i, j]);
					//if (gameScreen[i, j] >= 0x3000)
					//{
					//	drawSpecialChar(gameScreen[i, j], j);
					//}
					//else
					//{
					//	Console.Write((char)gameScreen[i, j]);
					//}
				}
			}
		}

		public void ResetBuffer()
		{
			Console.SetCursorPosition(0, 0);
			for (int i = 0; i < Rows; i++)
			{
				for (int j = 0; j < Cols; j++)
				{
					gameScreen[i, j] = ' ';
				}
			}
		}

		public void SetChar(int r, int c, int ch)
		{
			gameScreen[r, c] = ch;
			if (c <= Cols - 1 && ch >= 0x3000)
			{
				gameScreen[r, c + 1] = EMPTY_CHAR;
			}
		}

		public void SetLine(int row, string str)
		{
			if (str.Length > Cols)
				throw new NotImplementedException();    // TODO

			for (int sPtr = 0, gPtr = 0; gPtr < Cols && sPtr < str.Length; sPtr++, gPtr++)
			{
				gameScreen[row, gPtr] = str[sPtr];
				if (str[sPtr] >= 0x3000)
				{
					if (gPtr < Cols - 1)
					{
						gPtr++;
						gameScreen[row, gPtr] = EMPTY_CHAR;
					}
				}
			}
		}

		public void SetColumn(int col, string str)
		{
			if (str.Length > Rows)
				throw new NotImplementedException();

			// TODO: 如果穿过中文字符的后一格，需要将该中文字符也一并清除。

			for (int r = 0; r < str.Length; r++)
			{
				gameScreen[r, col] = str[r];
			}
		}

		#region TEST
		public void Test(int r, int c)
		{
			for(int i = r; i < r + 5; i++)
			{
				for (int j = c; j < c + 5; j++)
				{
					//用这个'█'就显示不了
					gameScreen[i, j] = '█';
				}
			}
		}
		#endregion
	}
}
