using System;
using System.IO;
using System.Threading;
using CMLMusicPlayer.Resources;
using Microsoft.Extensions.CommandLineUtils;

namespace CMLMusicPlayer.Arguments
{
	public class ArgParser
	{
		private readonly string[] args;

		private Version version;
		private string src;
		private int frameRate;
		private string errorMessage;	// give out the error message.
		
		private const int DEFAULT_FRAMERATE = 20;

		public ArgParser(string[] args)
		{
			this.args = args;

			if (args.Length == 0)
			{
				args = new[] { "Musics" };
			}

			ProcessArgs(args);

//			var player = new MusicPlayer(src)
//			{
//				Version = ver
//			};
//			player.Run();
	}

		public CMLConfig GetResult()
		{
			return new CMLConfig()
			{
				MusicFolder = src,
				FrameRate = frameRate
			};
		}

		private bool ProcessArgs(string[] args)
		{
			version = typeof(Program).Assembly.GetName().Version;

			var app = new CommandLineApplication(false)
			{
				Name = typeof(Program).Namespace,
				FullName = typeof(Program).Namespace,
				ShortVersionGetter = () => version.ToString(2),
				LongVersionGetter = () => version.ToString(3)
			};

			app.HelpOption("--help | -h");
			app.VersionOption("-v | --version", app.ShortVersionGetter, app.LongVersionGetter);

			var frameRateOpt = app.Option("-f | --frame", Strings.FrameRateOption, CommandOptionType.SingleValue);

//			var playOption = app.Option("-p | --play",
//				"Play all the musics in the specified music folder.", CommandOptionType.SingleValue);

			var pathArg = app.Argument("path", "The path of the music folder");

			frameRate = DEFAULT_FRAMERATE;

			app.OnExecute(() =>
			{
				if (frameRateOpt.HasValue())
				{
					frameRate = int.Parse(frameRateOpt.Value());
					if (frameRate <= 0 || frameRate > 60)
						frameRate = DEFAULT_FRAMERATE;
				}

				src = pathArg.Value;
				if (string.IsNullOrWhiteSpace(src))
				{
					//Console.WriteLine(Strings.PathNotSpecified);
					//src = Path.Combine(Directory.GetCurrentDirectory(), "Musics");
					//Directory.CreateDirectory(src);
					return -1;
				}

				if (!Directory.Exists(src))
				{
					Console.WriteLine(Strings.WrongPath);
					src = "Musics";
					Directory.CreateDirectory(src);
				}
				return 0;
			});

			int status = app.Execute(args);

			if (status != 0)
			{
				return false;
			}

			if (string.IsNullOrWhiteSpace(src))
			{
				return false;
			}
			
			Thread.Sleep(1000);

			return true;
		}

		public static int CheckFrameRate(int frameRate)
		{
			if (frameRate >= 1 && frameRate <= 60)
			{
				return frameRate;
			}

			return DEFAULT_FRAMERATE;
		}
	}
}
