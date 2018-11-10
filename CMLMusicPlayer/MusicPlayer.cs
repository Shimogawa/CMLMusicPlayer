using System;
using System.Globalization;
using System.Threading;
using Timer = System.Timers.Timer;
using CMLMusicPlayer.Resources;
using NAudio.Wave;
using System.Timers;
using System.IO;
using System.Collections.Generic;

namespace CMLMusicPlayer
{
    class MusicPlayer
    {
        //private const long Jan1st1970 = 621355968000000000L;

        private readonly Timer timer = new Timer(1000d);

        private bool musicEnd = true;

        private int frames = 0;
        private DateTime startOn;
        private DateTime lastFrame;

        public string PlaySrc { get; set; }
        public IEnumerable<string> files;

        public Version Version { get; set; }

        public int FR { get; set; }

        public MusicPlayer() { }

        public MusicPlayer(string src) : this()
        {
            PlaySrc = src;
            files = Directory.EnumerateFiles(src);
        }

        public void Run()
        {
            //player.FileName = Path.Combine(PlaySrc, "example.mp3");
            //player.FileName = "example.mp3";
            //player.Play();
            //DrawCredits();


            //timer.Interval = 1000.0 / FR;
            //timer.Elapsed += Task;
            //timer.AutoReset = true;

            //startOn = DateTime.UtcNow;
            //lastFrame = DateTime.UtcNow;
            //timer.Enabled = true;

            //while (timer.Enabled)
            //{
            //    if (Console.ReadKey(true).Key == ConsoleKey.Q)
            //        Environment.Exit(0);
            //}
            foreach (var file in files)
            {
                using (var audioFile = new AudioFileReader(file))
                using (var outputDevice = new WaveOutEvent() { Volume = 10 })
                {
                    outputDevice.Init(audioFile);
                    outputDevice.Play();
                    while (outputDevice.PlaybackState == PlaybackState.Playing)
                    {
                        Thread.Sleep(1000);
                    }
                }
            }
        }

        private void Task(object source, ElapsedEventArgs e)
        {
            frames++;
            if (musicEnd)
                DrawList();
            DrawProgress();

        }

        private void DrawList()
        {
            Console.Clear();
            Console.WriteLine("Draw list");
            musicEnd = false;
        }

        private void DrawProgress()
        {
            int height = Console.WindowHeight;
            if (System.Threading.Thread.CurrentThread.CurrentUICulture.Equals(CultureInfo.GetCultureInfo("zh-CN")))
                height--;
            Console.SetCursorPosition(0, height - 1);
            
            Console.Write("Draw Prog, Avg FR: {0:F2}, Cur FR: {1:F2}\r",
                FrameRate1(), FrameRate2());
        }

        private void DrawCredits()
        {
            Console.Clear();
            Console.WriteLine(Strings.Credits, Version.ToString());
            System.Threading.Thread.Sleep(2000);
        }

        private double FrameRate1()
        {
            var r = (DateTime.UtcNow - startOn).TotalMilliseconds;

            return 1000.0 / (r / frames);
        }

        private double FrameRate2()
        {
            var r = (DateTime.UtcNow - lastFrame).TotalMilliseconds;
            lastFrame = DateTime.UtcNow;
            return 1000.0 / r;
        }
    }
}
