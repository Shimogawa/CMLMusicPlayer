using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CMLMusicPlayer
{
	public class KeyBoardHandler
	{
		private Thread keyboardThread;
		private Dictionary<ConsoleKey, Action> keyEvents;

		public KeyBoardHandler()
		{
			keyboardThread = new Thread(keyboardControl);
			keyEvents = new Dictionary<ConsoleKey, Action>();
		}

		public void Register(ConsoleKey key, Action action)
		{
			keyEvents.Add(key, action);
		}

		public void Start()
		{
			keyboardThread.Start();
		}

		private void keyboardControl()
		{
			while (CMLApplication.Me.IsEnabled)
			{
				var key = Console.ReadKey(true).Key;
				if (keyEvents.ContainsKey(key))
				{
					keyEvents[key].Invoke();
				}
			}
		}
	}
}
