using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMLMusicPlayer.UI
{
	public class CoordMapper
	{
		public int MaxX
		{
			get;
		}
		public int MaxY
		{
			get;
		}
		private bool[,] coord;

		public CoordMapper(int maxX, int maxY)
		{
			MaxX = maxX;
			MaxY = maxY;
			coord = new bool[maxX, maxY];
			Reset();
		}

		public void Reset()
		{
			for (int i = 0; i < MaxX; i++)
			{
				for (int j = 0; j < MaxY; j++)
				{
					coord[i, j] = false;
				}
			}
		}

		/// <summary>
		/// Get the starting position of a character
		/// </summary>
		/// <returns></returns>
		public Coordinate<int> QueryCoord(int x, int y)
		{
			if(x >= MaxX || y >= MaxY || x < 0 || y < 0)
			{
				throw new ArgumentOutOfRangeException("Coordinate of given character is out of range!");
			}
			Coordinate<int> result = new Coordinate<int>(0, y);
			for (int i = 1; i <= x; i++)
			{
				if (coord[i, y])
					result.X++;
				result.X++;
			}
			if(result.X < 0 || result.X >= MaxX || result.Y < 0 || result.Y > MaxY)
			{
				throw new ArgumentOutOfRangeException("Coordinate of given character is out of range!");
			}
			return result;
		}

		/// <summary>
		/// Mark the position as 2 block
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void Set(int x, int y)
		{
			coord[x, y] = true;
		}

		/// <summary>
		/// Clear the position if it is marked with 2 block
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void Clear(int x, int y)
		{
			coord[x, y] = false;
		}


	}
}
