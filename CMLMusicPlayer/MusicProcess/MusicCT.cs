using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace CMLMusicPlayer.MusicProcess
{
    class MusicCT
    {
        private static MusicCT instance = new MusicCT();

        // API vars
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        private string Name = "";
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        private string DurLength = "";
        [MarshalAs(UnmanagedType.LPTStr, SizeConst = 128)]
        private string TemStr = "";

        private int ilong;

        // state enum
        private enum State
        {
            mPlaying = 1,
            mPuase = 2,
            mStop = 3
        };

        // structure
        private struct StructMCI
        {
            public bool bMut;
            public int iDur;
            public int iPos;
            public int iVol;
            public int iBal;
            public string iName;
            public State state;
        };

        private StructMCI mc = new StructMCI();

        public string FileName
        {
            get
            {
                return mc.iName;
            }
            set
            {
                //ASCIIEncoding asc = new ASCIIEncoding(); 
                TemStr = "";
                TemStr = TemStr.PadLeft(127, Convert.ToChar(" "));
                Name = Name.PadLeft(260, Convert.ToChar(" "));
                mc.iName = value;
                ilong = MusicAPI.GetShortPathName(mc.iName, Name, Name.Length);
                Name = GetCurrPath(Name);
                //Name = "open " + Convert.ToChar(34) + Name + Convert.ToChar(34) + " alias media";
                Name = "open " + Convert.ToChar(34) + Name + Convert.ToChar(34) + " alias media";
                ilong = MusicAPI.mciSendString("close all", TemStr, TemStr.Length, 0);
                ilong = MusicAPI.mciSendString(Name, TemStr, TemStr.Length, 0);
                ilong = MusicAPI.mciSendString("set media time format milliseconds", TemStr, TemStr.Length, 0);
                mc.state = State.mStop;
            }
        }

        private MusicCT() { }

        public void Play()
        {
            TemStr = "";
            TemStr = TemStr.PadLeft(127, Convert.ToChar(" "));
            MusicAPI.mciSendString("play media", TemStr, TemStr.Length, 0);
            mc.state = State.mPlaying;
        }
        
        public void Stop()
        {
            TemStr = "";
            TemStr = TemStr.PadLeft(128, Convert.ToChar(" "));
            ilong = MusicAPI.mciSendString("close media", TemStr, 128, 0);
            ilong = MusicAPI.mciSendString("close all", TemStr, 128, 0);
            mc.state = State.mStop;
        }

        public void Pause()
        {
            TemStr = "";
            TemStr = TemStr.PadLeft(128, Convert.ToChar(" "));
            ilong = MusicAPI.mciSendString("pause media", TemStr, TemStr.Length, 0);
            mc.state = State.mPuase;
        }

        private string GetCurrPath(string name)
        {
            name = name.Trim();
            if (name.Length < 1) return "";
            name = name.Substring(0, name.Length - 1);
            return name;
        }

        public int Duration
        {
            get
            {
                DurLength = "";
                DurLength = DurLength.PadLeft(128, Convert.ToChar(" "));
                MusicAPI.mciSendString("status media length", DurLength, DurLength.Length, 0);
                DurLength = DurLength.Trim();
                if (DurLength == "") return 0;
                return (int)(Convert.ToDouble(DurLength) / 1000f);
            }
        }

        //当前时间
        public int CurrentPosition
        {
            get
            {
                DurLength = "";
                DurLength = DurLength.PadLeft(128, Convert.ToChar(" "));
                MusicAPI.mciSendString("status media position", DurLength, DurLength.Length, 0);
                mc.iPos = (int)(Convert.ToDouble(DurLength) / 1000f);
                return mc.iPos;
            }
        }

        public static MusicCT GetInstance()
        {
            return instance;
        }
    }
}
