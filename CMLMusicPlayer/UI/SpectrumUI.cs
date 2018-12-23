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
		private Complex[] fftData;
		private double currentMax;
		public SpectrumUI(int Xlim, int Ylim)
		{
			xLimit = Xlim;
			yLimit = Ylim;
		}
		public void Update(Complex[] data)
		{
			fftData = data;
	
		}

		private int getYPos(double d)
		{
			double scale = d / currentMax;
			return yLimit - 1 - (int)(scale * (yLimit - 1));
		}

		private double getValue(Complex c)
		{
			return Math.Sqrt(c.X * c.X + c.Y * c.Y);
		}

		public void Draw(Renderer renderer)
		{
			if (fftData == null)
				return;
			currentMax = 0;
			int step = fftData.Length / xLimit + 1;
			double[] range = new double[xLimit];
			for (int i = 0; i < xLimit; i++)
			{
				range[i] = 0.0;
			}
			int k = 0;
			foreach (var c in fftData)
			{
				double l = getValue(c);
				range[k / step] += l / step;
				k++;
			}
			foreach (var d in range)
			{
				currentMax = Math.Max(currentMax, d);
			}
			for (int i = 0; i < xLimit; i++)
			{
				int y = getYPos(range[i]);
				if (y < 0)
					continue;
				for (int j = yLimit - 1; j >= y; j--)
				{
					renderer.SetChar(i, j, 'A');
				}
			}
		}
	}
}
