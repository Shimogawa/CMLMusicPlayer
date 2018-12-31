using System;
using System.Text;

namespace CMLMusicPlayer.UI.Main
{
	class MainUI : UIPanel
	{
		private int _x;
		private int _y;

		private SongListUI songList;

		public MainUI()
		{
		}

		public override void Update()
		{
			throw new NotImplementedException();
		}

		public override void Draw(Renderer renderer)
		{
			throw new NotImplementedException();
		}

		public override void SetPos(int x, int y)
		{
			_x = x;
			_y = y;
		}
	}
}
