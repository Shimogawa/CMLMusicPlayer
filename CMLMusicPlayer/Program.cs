using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;
using CMLMusicPlayer.Resources;


namespace CMLMusicPlayer
{
    class Program
    {
        private static string src;
        private static int frameRate = 60;
        private static Version ver;

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                args = new [] { "-h" };
                //args = new []
                //{
                //    "-p", src
                //};
            }

#if DEBUG
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("zh-CN");
#endif

            if (!ProcessArgs(args))
            {
                return;
            }

            var player = new MusicPlayer(src)
            {
                Version = ver,
                FR = frameRate
            };
            player.Run();

            //if (Process.GetProcessById())
        }

        private static bool ProcessArgs(string[] args)
        {
            ver = typeof(Program).Assembly.GetName().Version;

            var app = new CommandLineApplication(false)
            {
                Name = typeof(Program).Namespace,
                FullName = typeof(Program).Namespace,
                ShortVersionGetter = () => ver.ToString(2),
                LongVersionGetter = () => ver.ToString(3)
            };

            app.HelpOption("--help | -h");
            app.VersionOption("-v | --version", app.ShortVersionGetter, app.LongVersionGetter);

            var frameRateOpt = app.Option("-f | --frame", Strings.FrameRateOption, CommandOptionType.SingleValue);

            //var playOption = app.Option("-p | --play",
            //    "Play all the musics in the specified music folder.", CommandOptionType.SingleValue);

            var pathArg = app.Argument("path", "The path of the music folder");

            frameRate = 60;

            app.OnExecute(() =>
            {
                if (frameRateOpt.HasValue())
                {
                    try
                    {
                        frameRate = int.Parse(frameRateOpt.Value());
                        if (frameRate <= 0 || frameRate > 300)
                            throw new Exception();
                    }
                    catch (Exception)
                    {
                    }
                }
                
                src = pathArg.Value;
                //if (string.IsNullOrWhiteSpace(src))
                //{
                //    //Console.WriteLine(Strings.PathNotSpecified);
                //    //src = Path.Combine(Directory.GetCurrentDirectory(), "Musics");
                //    //Directory.CreateDirectory(src);
                //    return -1;
                //}
                //else if (src.Length > 2 && src[1] == ':')
                //{
                //    if (!Directory.Exists(src))
                //    {
                //        Console.WriteLine(Strings.WrongPath);
                //        src = Path.Combine(Directory.GetCurrentDirectory(), "Musics");
                //        Directory.CreateDirectory(src);
                //    }
                //}
                //else
                //{
                //    src = Path.Combine(Directory.GetCurrentDirectory(), src);
                //    if (!Directory.Exists(src))
                //    {
                //        Console.WriteLine(Strings.PathNotFound);
                //        Directory.CreateDirectory(src);
                //    }
                //}

                return 0;
            });

            app.Execute(args);

            if (string.IsNullOrWhiteSpace(src))
            {
                return false;
            }
            else if (src.Length > 2 && src[1] == ':')
            {
                if (!Directory.Exists(src))
                {
                    Console.WriteLine(Strings.WrongPath);
                    src = Path.Combine(Directory.GetCurrentDirectory(), "Musics");
                    Directory.CreateDirectory(src);
                }
            }
            else
            {
                src = Path.Combine(Directory.GetCurrentDirectory(), src);
                if (!Directory.Exists(src))
                {
                    Console.WriteLine(Strings.PathNotFound);
                    Directory.CreateDirectory(src);
                }
            }
            Thread.Sleep(1000);

            return true;
        }
    }
}
