using System;
using System.Drawing;
using System.IO;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Roche.Properties;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using WinRT.Interop;
using static Windows.Win32.PInvoke;

namespace Roche
{
    public sealed partial class ShellWindow : Window
    {
        const int Width = 548;
        const int Height = 756;

        public static IntPtr Handle { get; private set; }

        public ShellWindow()
        {
            InitializeComponent();

            Handle = WindowNative.GetWindowHandle(this);
            Title = "Roche";

            var handle = (HWND)Handle;

            // Set icon
            using (var stream = new MemoryStream(Resources.Cockroach))
            {
                var smallIcon = new Icon(stream, 16, 16);
                SendMessage(handle, WM_SETICON, (nuint)ICON_SMALL, smallIcon.Handle);

                stream.Seek(0, SeekOrigin.Begin);

                var bigIcon = new Icon(stream, 32, 32);
                SendMessage(handle, WM_SETICON, (nuint)ICON_BIG, bigIcon.Handle);
            }

            // Set size
            var dpi = GetDpiForWindow(handle);
            var scale = dpi / 96.0;
            SetWindowPos(
                handle,
                HWND_TOP,
                0,
                0,
                (int)(Width * scale),
                (int)(Height * scale),
                SET_WINDOW_POS_FLAGS.SWP_NOMOVE);

            // Remove minimize and maximize buttons
            var style = (WINDOW_STYLE)GetWindowLong(handle, WINDOW_LONG_PTR_INDEX.GWL_STYLE);
            SetWindowLong(
                handle,
                WINDOW_LONG_PTR_INDEX.GWL_STYLE,
                (int)(style & ~(WINDOW_STYLE.WS_MINIMIZEBOX | WINDOW_STYLE.WS_MAXIMIZEBOX)));
        }

        public Frame Frame
            => _frame;
    }
}
