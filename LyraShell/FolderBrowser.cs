// Disclaimer and Copyright Information
// FolderBrowser
//
// All rights reserved.
//
// Written by Pardesi Services, LLC
// Version 1.0.0


using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Lyra2.LyraShell
{
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
	[ComVisible(true)]
	public class BROWSEINFO
	{
		public IntPtr hwndOwner;
		public IntPtr pidlRoot;
		public IntPtr pszDisplayName;
		public string lpszTitle;
		public int ulFlags;
		public IntPtr lpfn;
		public IntPtr lParam;
		public int iImage;
	}

	public class Win32SDK
	{
		[DllImport("shell32.dll", PreserveSig=true, CharSet=CharSet.Auto)]
		public static extern IntPtr SHBrowseForFolder(BROWSEINFO bi);

		[DllImport("shell32.dll", PreserveSig=true, CharSet=CharSet.Auto)]
		public static extern bool SHGetPathFromIDList(IntPtr pidl, IntPtr pszPath);

		[DllImport("shell32.dll", PreserveSig=true, CharSet=CharSet.Auto)]
		public static extern int SHGetSpecialFolderLocation(IntPtr hwnd, int csidl, ref IntPtr ppidl);
	}

	#region BrowseFlags

	[Flags, Serializable]
	public enum BrowseFlags
	{
		BIF_DEFAULT = 0x0000,
		/// <summary>
		/// Only return computers. If the user selects anything other than a computer,
		/// the OK button is grayed.
		/// </summary>
		BIF_BROWSEFORCOMPUTER = 0x1000,
		/// <summary>
		/// Only return printers. If the user selects anything other than a printer,
		/// the OK button is grayed.  
		/// </summary>
		BIF_BROWSEFORPRINTER = 0x2000,
		/// <summary>
		///   Version 4.71. The browse dialog box will display files as well as folders.
		/// </summary>
		BIF_BROWSEINCLUDEFILES = 0x4000,
		/// <summary>
		///   Version 5.0. The browse dialog box can display URLs. The BIF_USENEWUI
		///   and BIF_BROWSEINCLUDEFILES flags must also be set. If these three flags are
		///   not set, the browser dialog box will reject URLs. Even when these flags are
		///   set, the browse dialog box will only display URLs if the folder that contains
		///   the selected item supports them. When the folder's IShellFolder::GetAttributesOf
		///   method is called to request the selected item's attributes, the folder must set
		///   the SFGAO_FOLDER attribute flag. Otherwise, the browse dialog box will not
		///   display the URL.
		/// </summary>
		BIF_BROWSEINCLUDEURLS = 0x0080,
		/// <summary>
		///   Do not include network folders below the domain level in the dialog box's
		///   tree view control.
		/// </summary>
		BIF_DONTGOBELOWDOMAIN = 0x0002,
		/// <summary>
		///   Version 4.71. Include an edit control in the browse dialog box that allows
		///   the user to type the name of an item.
		/// </summary>
		BIF_EDITBOX = 0x0010,
		/// <summary>
		/// Version 5.0. Use the new user interface. Setting this flag provides the user
		/// with a larger dialog box that can be resized. The dialog box has several
		/// new capabilities including: drag and drop capability within the dialog box,
		/// reordering, shortcut menus, new folders, delete, and other shortcut menu
		/// commands. To use this flag, you must call OleInitialize or CoInitialize
		/// before calling SHBrowseForFolder.
		/// </summary>
		BIF_NEWDIALOGSTYLE = 0x0040,
		/// <summary>
		///   Do not include the "New Folder" button in the browse dialog box.
		/// </summary>
		BIF_NONEWFOLDERBUTTON = 0x0200,
		/// <summary>
		///   Only return file system ancestors. An ancestor is a subfolder that is
		///   beneath the root folder in the namespace hierarchy. If the user selects
		///   an ancestor of the root folder that is not part of the file system,
		///   the OK button is grayed.
		/// </summary>
		BIF_RETURNFSANCESTORS = 0x0008,
		/// <summary>
		/// Only return file system directories. If the user selects folders that
		/// are not part of the file system, the OK button is grayed.
		/// </summary>
		BIF_RETURNONLYFSDIRS = 0x0001,
		/// <summary>
		///   Version 5.0. The browse dialog box can display shareable resources on
		///   remote systems. It is intended for applications that want to expose
		///   remote shares on a local system. The BIF_USENEWUI flag must also be set.
		/// </summary>
		BIF_SHAREABLE = 0x8000,
		/// <summary>
		/// Include a status area in the dialog box. The callback function can set
		/// the status text by sending messages to the dialog box.  
		/// </summary>
		BIF_STATUSTEXT = 0x0004,
		/// <summary>
		///  When combined with BIF_NEWDIALOGSTYLE, adds a usage hint to the dialog
		///  box in place of the edit box. BIF_EDITBOX overrides this flag.  
		/// </summary>
		BIF_UAHINT = 0x0100,
		/// <summary>
		///   Version 4.71. If the user types an invalid name into the edit box,
		///   the browse dialog box will call the application's BrowseCallbackProc
		///   with the BFFM_VALIDATEFAILED message. This flag is ignored if
		///   BIF_EDITBOX is not specified. 
		/// </summary>
		BIF_VALIDATE = 0x0020,
		/// <summary>
		/// don't traverse target as shortcut
		/// </summary>
		BIF_NOTRANSLATETARGETS = 0x0400,
	}

	#endregion

	/// <summary>
	/// Summary description for UserControl1.
	/// </summary>
	public class FolderBrowser : Component
	{
		private string m_strDirectoryPath;
		private string m_strTitle;
		private string m_strDisplayName;
		private BrowseFlags m_Flags;

		public FolderBrowser()
		{
			m_Flags = BrowseFlags.BIF_DEFAULT;
			m_strTitle = "";
		}

		/// <summary>
		/// This property gets the full path of the folder that has been selected.
		/// This value will only be set if user did not hit Cancel from dialog
		/// browser. Otherwise it return empty string.
		/// </summary>
		public string DirectoryPath
		{
			get { return this.m_strDirectoryPath; }
		}

		/// <summary>
		/// This property gets the display name of the folder that has been selected
		/// from folder browser. If user cancels out of the browser, then this
		/// property will return empty string.
		/// </summary>
		public string DisplayName
		{
			get { return this.m_strDisplayName; }
		}

		/// <summary>
		/// This property sets the Title that you want to set for this browser
		/// dialog box.
		/// </summary>
		public string Title
		{
			set { this.m_strTitle = value; }
		}

		/// <summary>
		/// This property sets the flags for display of browser dialog box.
		/// </summary>
		public BrowseFlags Flags
		{
			set { this.m_Flags = value; }
		}

		/// <summary>
		/// Call this method you have set the title, flags, etc. This will bring up
		/// the browser dialog box. Pick the folder you want to select and hit OK. If
		/// user clicks on Cancel button then no values for directory path, etc. are
		/// returned. If user hits OK and not errors occur, then the reurn value from 
		/// the method id DialogResult.OK otherwise it can be DialogResult.Cancel or
		/// DialogResult.Abort.
		/// </summary>
		/// <returns></returns>
		public DialogResult ShowDialog()
		{
			// Create
			BROWSEINFO bi = new BROWSEINFO();
			bi.pszDisplayName = IntPtr.Zero;
			bi.lpfn = IntPtr.Zero;
			bi.lParam = IntPtr.Zero;
			bi.lpszTitle = "Pfad für das lyra HTML-Verzeichnis...";
			IntPtr idListPtr = IntPtr.Zero;
			IntPtr pszPath = IntPtr.Zero;
			try
			{
				if (this.m_strTitle.Length != 0)
				{
					bi.lpszTitle = this.m_strTitle;
				}
				bi.ulFlags = (int) this.m_Flags;
				bi.pszDisplayName = Marshal.AllocHGlobal(256);
				// Call SHBrowseForFolder
				idListPtr = Win32SDK.SHBrowseForFolder(bi);
				// Check if the user cancelled out of the dialog or not.
				if (idListPtr == IntPtr.Zero)
				{
					return DialogResult.Cancel;
				}

				// Allocate ncessary memory buffer to receive the folder path.
				pszPath = Marshal.AllocHGlobal(256);
				// Call SHGetPathFromIDList to get folder path.
				// Convert the returned native poiner to string.
				m_strDirectoryPath = Marshal.PtrToStringAuto(pszPath);
				this.m_strDisplayName = Marshal.PtrToStringAuto(bi.pszDisplayName);
			}
			catch (Exception ex)
			{
				Trace.WriteLine(ex.Message);
				return DialogResult.Abort;
			}
			finally
			{
				// Free the memory allocated by shell.
				if (idListPtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(idListPtr);
				}
				if (pszPath != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(pszPath);
				}
				if (bi != null)
				{
					Marshal.FreeHGlobal(bi.pszDisplayName);
				}
			}
			return DialogResult.OK;
		}
	}
}