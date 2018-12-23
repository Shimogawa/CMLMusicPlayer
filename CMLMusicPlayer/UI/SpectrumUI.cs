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
			currentMax = 0;

		}

		private int getYPos(Complex c)
		{
			double l = Math.Sqrt(c.X * c.X + c.Y * c.Y);
			double scale = l / currentMax;
			return yLimit - 1 - (int)(scale * (yLimit - 1));
		}

		public void Draw(Renderer renderer)
		{
			if (fftData == null)
				return;
			int step = fftData.Length / xLimit + 1;
			double[] range = new double[xLimit];
			for (int i = 0; i < xLimit; i++)
			{
				range[i] = 0.0;
			}
			int k = 0;
			foreach (var c in fftData)
			{
				double l = Math.Sqrt(c.X * c.X + c.Y * c.Y);
				currentMax = Math.Max(currentMax, l);
				range[k / step] += getYPos(fftData[k]) / (double)step;
				k++;
			}
			for (int i = 0; i < xLimit; i++)
			{
				renderer.SetChar(i, (int)(range[i]), '*');
			}
		}
	}
}
