using System;
using System.Runtime.InteropServices;
using UnityEngine;

public static class BorderlessHelper
{
    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

    [DllImport("user32.dll")]
    private static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    private const int GWL_STYLE = -16;
    private const uint WS_POPUP = 0x80000000;
    private const uint WS_VISIBLE = 0x10000000;

    private static readonly IntPtr HWND_TOP = IntPtr.Zero;
    private const uint SWP_SHOWWINDOW = 0x0040;

    public static void MakeBorderlessFullscreen()
    {
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, false);
        Screen.fullScreen = false;

        IntPtr hwnd = GetActiveWindow();
        uint style = GetWindowLong(hwnd, GWL_STYLE);
        SetWindowLong(hwnd, GWL_STYLE, WS_POPUP | WS_VISIBLE);
        SetWindowPos(hwnd, HWND_TOP, 0, 0, Screen.currentResolution.width, Screen.currentResolution.height, SWP_SHOWWINDOW);
    }
}
