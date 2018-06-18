using System;
using System.Globalization;
using System.Timers;
using CMLMusicPlayer.MusicProcess;
using CMLMusicPlayer.Resources;

namespace CMLMusicPlayer
{
    class MusicPlayer
    {
        private Timer timer = new Timer(1000d);
        private MusicCT player = MusicCT.GetInstance();

        private bool musicEnd = true;

        private int testi = 0;

        public string PlaySrc { get; set; }

        public MusicPlayer() { }

        public MusicPlayer(string src) : this()
        {
            PlaySrc = src;
        }

        public void Run()
        {
            player.FileName = "";
            player.Play();
            DrawCredits();

            timer.Interval = 1000d / 60d;
            timer.Elapsed += Task;
            timer.AutoReset = true;
            timer.Enabled = true;

            while (timer.Enabled)
            {
                if (Console.ReadKey(true).Key == ConsoleKey.Q)
                    Environment.Exit(0);
            }
        }

        private void Task(object source, ElapsedEventArgs e)
        {
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
            
            Console.Write("Draw Prog{0}\r", ++testi);
        }

        private void DrawCredits()
        {
            Console.Clear();
            Console.WriteLine(Strings.Credits);
            System.Threading.Thread.Sleep(2000);
        }
    }
}
