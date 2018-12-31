
namespace CMLMusicPlayer.UI.Main
{
	public abstract class UIPanel : Shape
	{
		/// <summary>
		/// Used when needed to update. For example, SongListUI is updated every time song switches.
		/// </summary>
		public abstract void Update();
	}
}
