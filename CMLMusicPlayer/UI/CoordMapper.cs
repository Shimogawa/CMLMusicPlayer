using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMLMusicPlayer.UI
{
	public class CoordMapper
	{
		private readonly int maxX;
		private readonly int maxY;
		private bool[,] coord;

		public CoordMapper(int maxX, int maxY)
		{
			this.maxX = maxX;
			this.maxY = maxY;
			coord = new bool[maxX, maxY];
			Reset();
		}

		public void Reset()
		{
			for (int i = 0; i < maxX; i++)
			{
				for (int j = 0; j < maxY; j++)
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
			Coordinate<int> result = new Coordinate<int>(0, y);
			for (int i = 1; i <= x; i++)
			{
				if (coord[i, y])
					result.X++;
				result.X++;
			}
			if(result.X >= maxX || result.Y > maxY)
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
