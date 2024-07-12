using System.Runtime.InteropServices;

namespace Pomodorek.Platforms.Windows.Helpers;

public static class WindowHelper
{
    private const uint SWP_NOSIZE = 0x0001;
    private const uint SWP_NOMOVE = 0x0002;
    private const uint SWP_SHOWWINDOW = 0x0040;

    private readonly static IntPtr HWND_TOPMOST = new(-1);

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr windowHandle, int nCmdShow);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetForegroundWindow(IntPtr windowHandle);

    [DllImport("user32.dll")]
    private static extern bool SetWindowPos(IntPtr windowHandle, IntPtr windowHandleX, int x, int y, int cx, int cy, uint uflags);

    public static void ActivateWindow(MauiWinUIWindow window)
    {
        // Get window handle.
        var windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(window);

        // Restore window if minimized.
        ShowWindow(windowHandle, 0x00000009);

        // Set window position and bring it to the top.
        SetWindowPos(windowHandle, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_SHOWWINDOW);

        // Set window to foreground.
        SetForegroundWindow(windowHandle);
    }
}