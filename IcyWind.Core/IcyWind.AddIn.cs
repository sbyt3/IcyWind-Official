using System;
using System.AddIn;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using IcyWind.AddInViews;
using IcyWind.Core.Logic;
using IcyWind.Core.Logic.IcyWind;
using IcyWind.Core.Logic.Riot;
using IcyWind.Core.Logic.Riot.Update;
using IcyWind.Core.Pages;

namespace IcyWind.Core
{
    [AddIn("IcyWind.Core", Description = "The brains of IcyWind", Publisher = "Edward Vo (eddy5641)",
        Version = "0.1.0-Beta")]
    public class IcyWind : IMainView
    {
        public FrameworkElement Run(params object[] data)
        {
            //Create the main view
            UserInterfaceCore.HolderPage = new HolderPage();

            //Get the language
            UserInterfaceCore.SetResource(data[0].ToString());
            StaticVars.IcyWindLocation = System.IO.Path.GetDirectoryName((string) data[1]);

            UserInterfaceCore.Focus = () => FlashWindow.SetForegroundWindow((IntPtr)data[2]);
            UserInterfaceCore.Flash = () => FlashWindow.Flash((IntPtr)data[2], 20);

            //Look for the latest system yaml files
            RiotUpdateHandler.GetLatestSystemYaml();
            //RiotUpdateHandler.GetLatestSystemYamlPbe();

            //Change the view the main view has
            UserInterfaceCore.ChangeView(typeof(LoginPage));
            //Return the main view to base application
            return UserInterfaceCore.HolderPage;
        }

        public bool Close()
        {
            if (StaticVars.UserClientList.Any(x => x.IsInGame || x.IsInChampSelect || x.IsInQueue))
            {
                return true;
            }

            foreach (var account in StaticVars.UserClientList)
            {
                if (account.IsConnectedToRtmp)
                {
                    account.HeartbeatTimer.Stop();
                    ((RiotCalls) account).Logout(account.RiotSession);
                    account.RiotConnection.CloseAsync();
                }

                new Thread(() =>
                {
                    try
                    {
                        account.XmppClient.Disconnect();
                    }
                    catch
                    {
                        //Ignored
                    }

                }).Start();

                account.HasLoggedOut = true;
            }

            //Y u no close if you not here?
            Environment.Exit(0);
            return false;
        }
    }


    #region --- FlashWindow-stuff ---

    // 11-01-12 Olaf:
    // Modified version of the stuff provided for WinForms to work with WPF.
    // Source: http://pietschsoft.com/post/2009/01/26/CSharp-Flash-Window-in-Taskbar-via-Win32-FlashWindowEx.aspx

    // ReSharper disable InconsistentNaming
    // ReSharper disable IdentifierTypo
    public static class FlashWindow
    {

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [StructLayout(LayoutKind.Sequential)]
        private struct FLASHWINFO
        {
            /// <summary>
            /// The size of the structure in bytes.
            /// </summary>
            public uint cbSize;

            /// <summary>
            /// A Handle to the Window to be Flashed. The window can be either opened or minimized.
            /// </summary>
            public IntPtr hwnd;

            /// <summary>
            /// The Flash Status.
            /// </summary>
            public uint dwFlags;

            /// <summary>
            /// The number of times to Flash the window.
            /// </summary>
            public uint uCount;

            /// <summary>
            /// The rate at which the Window is to be flashed, in milliseconds. If Zero, the function uses the default cursor blink rate.
            /// </summary>
            public uint dwTimeout;
        }

        /// <summary>
        /// Stop flashing. The system restores the window to its original stae.
        /// </summary>
        public const uint FLASHW_STOP = 0;

        /// <summary>
        /// Flash the window caption.
        /// </summary>
        public const uint FLASHW_CAPTION = 1;

        /// <summary>
        /// Flash the taskbar button.
        /// </summary>
        public const uint FLASHW_TRAY = 2;

        /// <summary>
        /// Flash both the window caption and taskbar button.
        /// This is equivalent to setting the FLASHW_CAPTION | FLASHW_TRAY flags.
        /// </summary>
        public const uint FLASHW_ALL = 3;

        /// <summary>
        /// Flash continuously, until the FLASHW_STOP flag is set.
        /// </summary>
        public const uint FLASHW_TIMER = 4;

        /// <summary>
        /// Flash continuously until the window comes to the foreground.
        /// </summary>
        public const uint FLASHW_TIMERNOFG = 12;


        /// <summary>
        /// Flash the spacified Window (Form) until it recieves focus.
        /// </summary>
        /// <param name="form">The Form (Window) to Flash.</param>
        /// <returns></returns>
        public static bool Flash(IntPtr hwnd)
        {
            // Make sure we're running under Windows 2000 or later
            if (Win2000OrLater)
            {
                FLASHWINFO fi = Create_FLASHWINFO(hwnd, FLASHW_ALL | FLASHW_TIMERNOFG, uint.MaxValue, 0);

                return FlashWindowEx(ref fi);
            }

            return false;
        }

        private static FLASHWINFO Create_FLASHWINFO(IntPtr handle, uint flags, uint count, uint timeout)
        {
            var fi = new FLASHWINFO();
            fi.cbSize = Convert.ToUInt32(Marshal.SizeOf(fi));
            fi.hwnd = handle;
            fi.dwFlags = flags;
            fi.uCount = count;
            fi.dwTimeout = timeout;
            return fi;
        }

        /// <summary>
        /// Flash the specified Window (form) for the specified number of times
        /// </summary>
        /// <param name="hwnd">The handle of the Window to Flash.</param>
        /// <param name="count">The number of times to Flash.</param>
        /// <returns></returns>
        public static bool Flash(IntPtr hwnd, uint count)
        {
            if (!Win2000OrLater) return false;
            var fi = Create_FLASHWINFO(hwnd, FLASHW_ALL, count, 0);

            return FlashWindowEx(ref fi);
        }

        /// <summary>
        /// Start Flashing the specified Window (form)
        /// </summary>
        /// <param name="form">The Form (Window) to Flash.</param>
        /// <returns></returns>
        public static bool Start(IntPtr handle)
        {
            if (Win2000OrLater)
            {
                FLASHWINFO fi = Create_FLASHWINFO(handle, FLASHW_ALL, uint.MaxValue, 0);
                return FlashWindowEx(ref fi);
            }

            return false;
        }

        /// <summary>
        /// Stop Flashing the specified Window (form)
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static bool Stop(IntPtr handle)
        {
            if (Win2000OrLater)
            {
                FLASHWINFO fi = Create_FLASHWINFO(handle, FLASHW_STOP, uint.MaxValue, 0);
                return FlashWindowEx(ref fi);
            }

            return false;
        }

        /// <summary>
        /// A boolean value indicating whether the application is running on Windows 2000 or later.
        /// </summary>
        private static bool Win2000OrLater => Environment.OSVersion.Version.Major >= 5;

        #endregion // <--- FlashWindow-stuff ---
    }
}