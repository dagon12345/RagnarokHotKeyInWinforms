using System.Runtime.InteropServices;
using System.Windows.Forms;
using System;

namespace Infrastructure.Utilities
{
    public class InteropUtility
    {
        //Pinvokes used in AUTOPOT
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, int Msg, Keys wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, int dwExtraInfo);
    }
}
