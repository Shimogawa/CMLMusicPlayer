
namespace CMLMusicPlayer.Arguments
{
	/// <summary>
	/// Used for the configuration of the app.
	/// </summary>
	public class CMLConfig
	{
		/// <summary>
		/// The music folder.
		/// </summary>
		public string MusicFolder { get; set; }

		/// <summary>
		/// The frame rate.
		/// </summary>
		public int FrameRate
		{
			get => ArgParser.CheckFrameRate(frameRate);
			set => frameRate = value;
		}

		private int frameRate;

		public CMLConfig()
		{
		}
	}
}
