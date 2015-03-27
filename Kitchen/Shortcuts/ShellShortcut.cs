/**************************************************************************
*
* Filename:     ShellShortcut.cs
* Author:       Mattias Sjögren (mattias@mvps.org)
*               http://www.msjogren.net/dotnet/
*
* Description:  Defines a .NET friendly class, ShellShortcut, for reading
*               and writing shortcuts.
*               Define the conditional compilation symbol UNICODE to use
*               IShellLinkW internally.
*
* Public types: class ShellShortcut
*
*
* Dependencies: ShellLinkNative.cs
*
*
* Copyright ©2001-2002, Mattias Sjögren
* 
**************************************************************************/

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Kitchen.Shortcuts
{
    /// <summary>
    ///   .NET friendly wrapper for the ShellLink class
    /// </summary>
    public class ShellShortcut : IDisposable
    {
        private const int INFOTIPSIZE = 1024;
        private const int MAX_PATH = 260;

        private const int SW_SHOWNORMAL = 1;
        private const int SW_SHOWMINIMIZED = 2;
        private const int SW_SHOWMAXIMIZED = 3;
        private const int SW_SHOWMINNOACTIVE = 7;

        private IShellLink m_Link;
        private readonly string m_sPath;

        ///
        /// <param name='linkPath'>
        ///   Path to new or existing shortcut file (.lnk).
        /// </param>
        ///
        public ShellShortcut(string linkPath)
        {
            m_sPath = linkPath;
            m_Link = (IShellLink)new ShellLink();

            if (!File.Exists(linkPath))
                return;

            try
            {
                ((IPersistFile)m_Link).Load(linkPath, 0);
            }
            catch (IOException)
            {
                // file could not be loaded (maybe shortcut doesn't exist any more).
            }
        }

        //
        //  IDisplosable implementation
        //
        public void Dispose()
        {
            if (m_Link == null)
                return;

            Marshal.ReleaseComObject(m_Link);
            m_Link = null;
        }

        /// <summary>
        ///   Gets or sets the argument list of the shortcut.
        /// </summary>
        public string Arguments
        {
            get
            {
                StringBuilder sb = new StringBuilder(INFOTIPSIZE);
                m_Link.GetArguments(sb, sb.Capacity);
                return sb.ToString();
            }
            set { m_Link.SetArguments(value); }
        }

        /// <summary>
        ///   Gets or sets a description of the shortcut.
        /// </summary>
        public string Description
        {
            get
            {
                StringBuilder sb = new StringBuilder(INFOTIPSIZE);
                m_Link.GetDescription(sb, sb.Capacity);
                return sb.ToString();
            }
            set { m_Link.SetDescription(value); }
        }

        /// <summary>
        /// Gets or sets the working directory (aka start-in directory) of the shortcut.
        /// </summary>
        public string WorkingDirectory
        {
            get
            {
                StringBuilder sb = new StringBuilder(MAX_PATH);
                m_Link.GetWorkingDirectory(sb, sb.Capacity);
                return sb.ToString();
            }
            set { m_Link.SetWorkingDirectory(value); }
        }

        //
        // If Path returns an empty string, the shortcut is associated with
        // a PIDL instead, which can be retrieved with IShellLink.GetIDList().
        // This is beyond the scope of this wrapper class.
        //
        /// <summary>
        ///   Gets or sets the target path of the shortcut.
        /// </summary>
        public string TargetPath
        {
            get
            {
                WIN32_FIND_DATAW wfd;

                var sb = new StringBuilder(MAX_PATH);

                m_Link.GetPath(sb, sb.Capacity, out wfd, SLGP_FLAGS.SLGP_UNCPRIORITY);
                return sb.ToString();
            }
            set { m_Link.SetPath(value); }
        }

        /// <summary>
        ///   Gets or sets the path of the <see cref="Icon"/> assigned to the shortcut.
        /// </summary>
        /// <summary>
        ///   <seealso cref="IconIndex"/>
        /// </summary>
        public string IconPath
        {
            get
            {
                string iconPath;
                int iconIndex;
                GetIconLocation(out iconPath, out iconIndex);
                return iconPath;
            }
            set { m_Link.SetIconLocation(value, IconIndex); }
        }

        /// <summary>
        ///   Gets or sets the index of the <see cref="Icon"/> assigned to the shortcut.
        ///   Set to zero when the <see cref="IconPath"/> property specifies a .ICO file.
        /// </summary>
        /// <summary>
        ///   <seealso cref="IconPath"/>
        /// </summary>
        public int IconIndex
        {
            get
            {
                string iconPath;
                int iconIndex;
                GetIconLocation(out iconPath, out iconIndex);
                return iconIndex;
            }
            set { m_Link.SetIconLocation(IconPath, value); }
        }

        /// <summary>
        ///   Retrieves the Icon of the shortcut as it will appear in Explorer.
        ///   Use the <see cref="IconPath"/> and <see cref="IconIndex"/>
        ///   properties to change it.
        /// </summary>
        public Icon Icon
        {
            get
            {
                string iconPath;
                int iconIndex;
                GetIconLocation(out iconPath, out iconIndex);

                // an empty iconPath means the shortcut has no custom icon set, so substitute the target's default icon.
                if (iconPath == string.Empty)
                    return Icon.ExtractAssociatedIcon(TargetPath);

                IntPtr hIcon = NativeMethods.ExtractIcon(IntPtr.Zero, iconPath, iconIndex);
                if (hIcon == IntPtr.Zero)
                    return null;

                // Return a cloned icon, because we have to free the original ourselves.
                Icon clone;
                using (Icon ico = Icon.FromHandle(hIcon))
                    clone = (Icon)ico.Clone();
               
                NativeMethods.DestroyIcon(hIcon);
                return clone;
            }
        }

        private void GetIconLocation(out string iconPath, out int iconIndex)
        {
            StringBuilder iconPathSB = new StringBuilder(MAX_PATH);
            m_Link.GetIconLocation(iconPathSB, iconPathSB.Capacity, out iconIndex);
            iconPath = iconPathSB.ToString();
        }

        /// <summary>
        ///   Gets or sets the System.Diagnostics.ProcessWindowStyle value
        ///   that decides the initial show state of the shortcut target. Note that
        ///   ProcessWindowStyle.Hidden is not a valid property value.
        /// </summary>
        public ProcessWindowStyle WindowStyle
        {
            get
            {
                int nWS;
                m_Link.GetShowCmd(out nWS);

                switch (nWS)
                {
                    case SW_SHOWMINIMIZED:
                    case SW_SHOWMINNOACTIVE:
                        return ProcessWindowStyle.Minimized;

                    case SW_SHOWMAXIMIZED:
                        return ProcessWindowStyle.Maximized;

                    default:
                        return ProcessWindowStyle.Normal;
                }
            }
            set
            {
                int nWS;

                switch (value)
                {
                    case ProcessWindowStyle.Normal:
                        nWS = SW_SHOWNORMAL;
                        break;

                    case ProcessWindowStyle.Minimized:
                        nWS = SW_SHOWMINNOACTIVE;
                        break;

                    case ProcessWindowStyle.Maximized:
                        nWS = SW_SHOWMAXIMIZED;
                        break;

                    default: // ProcessWindowStyle.Hidden
                        throw new ArgumentException("Unsupported ProcessWindowStyle value.");
                }

                m_Link.SetShowCmd(nWS);
            }
        }

        /// <summary>
        ///   Gets or sets the hotkey for the shortcut.
        /// </summary>
        public Keys Hotkey
        {
            get
            {
                short wHotkey;
                m_Link.GetHotkey(out wHotkey);

                //
                // Convert from IShellLink 16-bit format to Keys enumeration 32-bit value
                // IShellLink: 0xMMVK
                // Keys:  0x00MM00VK        
                //   MM = Modifier (Alt, Control, Shift)
                //   VK = Virtual key code
                //       
                int dwHotkey = ((wHotkey & 0xFF00) << 8) | (wHotkey & 0xFF);
                return (Keys)dwHotkey;
            }
            set
            {
                if ((value & Keys.Modifiers) == 0)
                    throw new ArgumentException("Hotkey must include a modifier key.");

                //    
                // Convert from Keys enumeration 32-bit value to IShellLink 16-bit format
                // IShellLink: 0xMMVK
                // Keys:  0x00MM00VK        
                //   MM = Modifier (Alt, Control, Shift)
                //   VK = Virtual key code
                //       
                short wHotkey = unchecked((short)(((int)(value & Keys.Modifiers) >> 8) | (int)(value & Keys.KeyCode)));
                m_Link.SetHotkey(wHotkey);
            }
        }

        /// <summary>
        ///   Saves the shortcut to disk.
        /// </summary>
        public void Save()
        {
            IPersistFile pf = (IPersistFile)m_Link;
            pf.Save(m_sPath, true);
        }

        /// <summary>
        ///   Returns a reference to the internal ShellLink object,
        ///   which can be used to perform more advanced operations
        ///   not supported by this wrapper class, by using the
        ///   IShellLink interface directly.
        /// </summary>
        public object ShellLink
        {
            get { return m_Link; }
        }

        #region Native Win32 API functions

        private static class NativeMethods
        {
            [DllImport("shell32.dll", CharSet = CharSet.Auto)]
            public static extern IntPtr ExtractIcon(IntPtr hInst, string lpszExeFileName, int nIconIndex);
            
            [DllImport("user32.dll")]
            public static extern bool DestroyIcon(IntPtr hIcon);
        }

        #endregion
    }
}