using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMLMusicPlayer.UI
{
	public struct Coordinate<T> where T : struct
	{
		public T X
		{
			get;
			set;
		}

		public T Y
		{
			get;
			set;
		}

		public Coordinate(T X, T Y)
		{
			this.X = X;
			this.Y = Y;
		}
	}
}
