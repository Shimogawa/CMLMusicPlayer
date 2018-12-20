using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMLMusicPlayer.Arguments
{
	/// <summary>
	/// 用于存储配置信息（文件名，……）
	/// </summary>
	public class CMLConfig
	{
		public string FileName { get; set; }
		public int FrameRate { get; set; }

		public CMLConfig()
		{

		}
	}
}
