using System;
using System.Runtime.InteropServices;

namespace CMLMusicPlayer.Utils
{
	public static class ConsoleUtil
	{
		/// <summary>
		/// Use this for <code>wFlags</code>.
		/// </summary>
		public const int MF_BYCOMMAND = 0x00000000;

		/// <summary>
		/// Menu for "Close"
		/// </summary>
		public const int SC_CLOSE = 0xF060;

		/// <summary>
		/// Menu for "Minimize"
		/// </summary>
		public const int SC_MINIMIZE = 0xF020;

		/// <summary>
		/// Menu for "Maximize"
		/// </summary>
		public const int SC_MAXIMIZE = 0xF030;

		/// <summary>
		/// Menu for "Resize"
		/// </summary>
		public const int SC_SIZE = 0xF000;

		/// <summary>
		/// Delete a function(menu) of the console window.
		/// </summary>
		/// <param name="hMenu">The console menu</param>
		/// <param name="nPosition">The type of menu deleting</param>
		/// <param name="wFlags">Use <code>MF_BYCOMMAND</code></param>
		/// <returns></returns>
		[DllImport("user32.dll")]
		public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

		/// <summary>
		/// Get the handle of console menu.
		/// </summary>
		/// <param name="hWnd">The handle of the console.</param>
		/// <param name="bRevert"></param>
		/// <returns></returns>
		[DllImport("user32.dll")]
		public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

		/// <summary>
		/// Get the console window handle.
		/// </summary>
		/// <returns></returns>
		[DllImport("kernel32.dll", ExactSpelling = true)]
		public static extern IntPtr GetConsoleWindow();

		/// <summary>
		/// Fixes the size of the console window.
		/// </summary>
		public static void FixConsoleWindowSize()
		{
			IntPtr handle = GetConsoleWindow();
			IntPtr sysMenu = GetSystemMenu(handle, false);

			if (handle == IntPtr.Zero)
			{
				throw new SystemException("Cannot get console window handle.");
			}
			DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);
		}

		/*
		 * Usage:
			IntPtr handle = GetConsoleWindow();
			IntPtr sysMenu = GetSystemMenu(handle, false);

			if (handle != IntPtr.Zero)
			{
				DeleteMenu(sysMenu, SC_CLOSE, MF_BYCOMMAND);		// No Closing
				DeleteMenu(sysMenu, SC_MINIMIZE, MF_BYCOMMAND);		// No minimizing
				DeleteMenu(sysMenu, SC_MAXIMIZE, MF_BYCOMMAND);		// No maximizing
				DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);			// No resizing
			}
			Console.Read();
		*/
	}
}
