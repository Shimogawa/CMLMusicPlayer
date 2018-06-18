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

            var ver = typeof(Program).Assembly.GetName().Version;

            var app = new CommandLineApplication(false)
            {
                Name = typeof(Program).Namespace,
                FullName = typeof(Program).Namespace,
                ShortVersionGetter = () => ver.ToString(2),
                LongVersionGetter = () => ver.ToString(3)
            };

            app.HelpOption("--help | -h");
            app.VersionOption("-v | --version | -version", app.ShortVersionGetter, app.LongVersionGetter);

            //var playOption = app.Option("-p | --play",
            //    "Play all the musics in the specified music folder.", CommandOptionType.SingleValue);

            var pathArg = app.Argument("path", "The path of the music folder");


            app.OnExecute(() =>
            {
                //if (playOption.HasValue())
                //{
                //    if (Directory.Exists(playOption.Value()))
                //    {
                //        src = playOption.Value();
                //    }
                //    else
                //    {
                //        Directory.CreateDirectory(src);
                //    }
                //}
                //else
                //{
                //    Directory.CreateDirectory(src);
                //}
                src = pathArg.Value;
                if (string.IsNullOrWhiteSpace(src))
                {
                    Console.WriteLine(Strings.PathNotSpecified);
                    src = Path.Combine(Directory.GetCurrentDirectory(), "Musics");
                }
                else if (src.Length > 2 && src[1] == ':')
                {
                    if (!Directory.Exists(src))
                    {
                        Console.WriteLine(Strings.WrongPath);
                        src = Path.Combine(Directory.GetCurrentDirectory(), "Musics");
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
                
                return 0;
            });

            app.Execute(args);

            var player = new MusicPlayer(src);
            player.Run();

            //if (Process.GetProcessById())
        }
    }
}
