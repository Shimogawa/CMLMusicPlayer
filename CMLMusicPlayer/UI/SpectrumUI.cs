using NAudio.Dsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMLMusicPlayer.UI
{
	public class SpectrumUI
	{
		private readonly int xLimit;
		private readonly int yLimit;
		private int updateCount;
		private int validRange;
		private int[] coords;

		public SpectrumUI(int Xlim, int Ylim, int size)
		{
			xLimit = Xlim;
			yLimit = Ylim;
			updateCount = 0;
			validRange = size / 2;
			coords = new int[Xlim + 10];
		}

		private double GetYPosLog(Complex c)
		{
			double intensityDB = 10 * Math.Log10(Math.Sqrt(c.X * c.X + c.Y * c.Y));
			double minDB = -90;
			if (intensityDB < minDB) intensityDB = minDB;
			double percent = intensityDB / minDB;
			double yPos = percent * yLimit;
			return yPos;
		}

		public void Update(Complex[] data)
		{
			try
			{
				if (updateCount++ % 2 == 0)
				{
					return;
				}
				int step = validRange / xLimit;
				for (int i = 0; i < data.Length / 2; i += step)
				{
					double yPos = 0;
					for (int b = 0; b < step; b++)
					{
						yPos += GetYPosLog(data[i + b]);
					}
					int id = i / step;
					coords[id] = (int)(yPos / step);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

		}


		private double getValue(Complex c)
		{
			return Math.Sqrt(c.X * c.X + c.Y * c.Y);
		}

		public void Draw(Renderer renderer)
		{ 
			for (int i = 0; i < xLimit; i++)
			{
				int y = coords[i];
				if (y < 0)
					continue;
				for (int j = yLimit - 1; j > y; j--)
				{
					renderer.SetChar(i, j, '*');
				}
			}
		}
	}
}
