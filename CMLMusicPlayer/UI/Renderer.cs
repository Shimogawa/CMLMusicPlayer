using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMLMusicPlayer.UI
{
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
		private char[,] gameScreen;

		public Renderer(int rows, int cols)
		{
			Rows = rows;
			Cols = cols;
			gameScreen = new char[rows, cols];

		}

		public void Present()
		{
			for(int i = 0; i < Rows; i++)
			{
				for(int j = 0; j < Cols; j++)
				{
					Console.SetCursorPosition(j, i);
					Console.Write(gameScreen[i, j]);
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

		public void SetChar(int r, int c, char ch)
		{
			gameScreen[r, c] = ch;
		}

		public void Test(int r, int c)
		{
			for(int i = r; i < r + 5; i++)
			{
				for (int j = c; j < c + 5; j++)
				{
					if (CMLApplication.Me.Random.Next(5) == 0)
					{
						//用这个'█'就显示不了
						gameScreen[i, j] = '*';
					}
				}
			}
		}
	}
}
