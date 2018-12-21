using Newtonsoft.Json;
using System.IO;

namespace CMLMusicPlayer.Arguments
{
	/// <summary>
	/// Used for the configuration of the app.
	/// </summary>
	public class CMLConfig
	{
		[JsonProperty("Music Folder")]
		/// <summary>
		/// The music folder.
		/// </summary>
		public string MusicFolder
		{
			get => musicFolder;
			set => musicFolder = value;
		}

		[JsonProperty("Frame Rate")]
		/// <summary>
		/// The frame rate.
		/// </summary>
		public int FrameRate
		{
			get => ArgParser.CheckFrameRate(frameRate);
			set => frameRate = value;
		}

		[JsonIgnore]
		private string musicFolder;

		[JsonIgnore]
		private int frameRate;

		public CMLConfig()
		{
		}

		public static CMLConfig Read(string path)
		{
			CMLConfig result;
			using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				using (var streamReader = new StreamReader(fileStream))
				{
					result = JsonConvert.DeserializeObject<CMLConfig>(streamReader.ReadToEnd());
				}
			}
			return result;
		}

		public static void Write(CMLConfig config, string path)
		{
			using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write))
			{
				string jStr = JsonConvert.SerializeObject(config, Formatting.Indented);
				using (var streamWriter = new StreamWriter(fileStream))
				{
					streamWriter.Write(jStr);
				}
			}
		}
	}
}
