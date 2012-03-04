using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Lyra2.LyraShell
{
	/// <summary>
	/// FileLauncher
	/// creates new process and opens the file with the Windows default application
	/// </summary>
	public class FileLauncher
	{
		// Win32 native error codes
		private const int ERROR_FILE_NOT_FOUND = 2;
		private const int ERROR_ACCESS_DENIED = 5;
		// IconSize
		public enum IconSize : uint
		{
			Small = 0x0, //16x16
			Large = 0x1 //32x32
		}

		public static void openFile(string filename)
		{
			Process myProcess = new Process();

			try
			{
				myProcess.StartInfo.FileName = filename;
				myProcess.Start();
			}
			catch (Win32Exception e)
			{
				if (e.NativeErrorCode == FileLauncher.ERROR_FILE_NOT_FOUND)
				{
					Util.MBoxError("Datei wurde nicht gefunden!");
				}
				else if (e.NativeErrorCode == FileLauncher.ERROR_ACCESS_DENIED)
				{
					Util.MBoxError("Datei-Zugriff wurde verweigert!");
				}
			}
		}

		// Extract Standard Icon
		[DllImport("Shell32", CharSet=CharSet.Auto)]
		internal extern static int ExtractIconEx(
			[MarshalAs(UnmanagedType.LPTStr)]
				string lpszFile, //size of the icon
			int nIconIndex, //index of the icon (in case we have more than 1 icon in the file
			IntPtr[] phIconLarge, //32x32 icon
			IntPtr[] phIconSmall, //16x16 icon
			int nIcons); //how many to get

		/// <summary>
		/// Get Windows-Icon for the Extension of a file
		/// </summary>
		/// <param name="Extension">ext of tile</param>
		/// <param name="Size">IconSize</param>
		/// <returns>Icon for this extension</returns>
		public static Icon IconFromExtension(string Extension, IconSize Size)
		{
			Icon icon = null;
			try
			{
				//add '.' if necessary
				if (Extension[0] != '.') Extension = '.' + Extension;

				//search registry for the file extension
				RegistryKey Root = Registry.ClassesRoot;
				RegistryKey ExtensionKey = Root.OpenSubKey(Extension);
				ExtensionKey.GetValueNames();
				RegistryKey appKey = Root.OpenSubKey(ExtensionKey.GetValue("").ToString());

				//gets the name of the file that has the icon.
				string IconLocation = appKey.OpenSubKey("DefaultIcon").GetValue("").ToString();
				string[] IconPath = IconLocation.Split(',');

				if (IconPath[1] == null) IconPath[1] = "0";
				IntPtr[] Large = new IntPtr[1];
				IntPtr[] Small = new IntPtr[1];

				//extracts the icon from the file.
				ExtractIconEx(IconPath[0], Convert.ToInt16(IconPath[1]), Large, Small, 1);
				icon = Size == IconSize.Large ? Icon.FromHandle(Large[0]) : Icon.FromHandle(Small[0]);
			}
			catch (Exception e)
			{
				Util.Debug("Icon konnte nicht extrahiert werden!", e);
			}
			return icon;
		}
	}
}