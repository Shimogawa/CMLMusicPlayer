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

		// override object.Equals
		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}
			Coordinate<T> that = (Coordinate<T>)obj;
			return X.Equals(that.X) && Y.Equals(that.Y);
		}

		// override object.GetHashCode
		public override int GetHashCode()
		{
			unchecked // Overflow is fine, just wrap
			{
				int hash = 17;
				hash = hash * 23 + X.GetHashCode();
				hash = hash * 23 + Y.GetHashCode();
				return hash;
			}
		}
	}
}
