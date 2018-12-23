using System;
using System.Globalization;
using System.IO;
using System.Threading;
using Microsoft.Extensions.CommandLineUtils;
using CMLMusicPlayer.Resources;
using CMLMusicPlayer.Arguments;


namespace CMLMusicPlayer
{
	public class Program
	{
		private static string src;
		private static Version ver;

		public static void Main(string[] args)
		{
			ArgParser argParser = new ArgParser(args);
			CMLConfig config = argParser.GetResult();
			CMLApplication application = new CMLApplication(config);
			application.Run();
			//CMLConfig.Write(config, "config.json");
		}
	}
}
