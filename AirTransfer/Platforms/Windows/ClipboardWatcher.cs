using System.Runtime.InteropServices;

using AirTransfer.Interfaces;

namespace AirTransfer;

public sealed class ClipboardWatcher : IClipboardWatchable
{
    private IntPtr _hwndNextViewer; // 剪贴板消息链的下一个窗口
    private IntPtr _hwnd; // 当前窗口句柄
    public Microsoft.UI.Xaml.Window _window;

    public void Initialize(object parameter)
    {
        // if (parameter is not Window window)
        //     throw new ArgumentException("Parameter must be of type Window");
        //
        // _window = window;

        var view = Application.Current.Windows[0].Handler.PlatformView as Microsoft.UI.Xaml.Window;
        // 获取当前窗口句柄
        _hwnd = WinRT.Interop.WindowNative.GetWindowHandle(view);

        // 将窗口添加到剪贴板消息链
        _hwndNextViewer = SetClipboardViewer(_hwnd);
        if (_hwndNextViewer == IntPtr.Zero)
        {
            var error = Marshal.GetLastWin32Error();
            var errorCode = GetLastError();
            Console.WriteLine($"SetClipboardViewer failed. Error code: {errorCode}");
        }

        // 注册窗口过程处理函数
        HwndSourceHook();
    }

    public void Dispose()
    {
        // 从剪贴板消息链移除
        ChangeClipboardChain(_hwnd, _hwndNextViewer);
    }

    private void HwndSourceHook()
    {
        // 通过 Win32 消息处理机制监控剪贴板消息
        Win32MessageManager.AddHook(_hwnd, WndProc);
    }

    private IntPtr WndProc(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam)
    {
        switch (msg)
        {
            case WM_DRAWCLIPBOARD:
                OnClipboardContentChanged();
                SendMessage(_hwndNextViewer, WM_DRAWCLIPBOARD, wParam, lParam);
                break;

            case WM_CHANGECBCHAIN:
                if (wParam == _hwndNextViewer)
                    _hwndNextViewer = lParam;
                else
                    SendMessage(_hwndNextViewer, WM_CHANGECBCHAIN, wParam, lParam);
                break;
        }

        return DefWindowProc(hwnd, msg, wParam, lParam);
    }

    private void OnClipboardContentChanged()
    {
        var content = GetClipboardText();
        ClipboardUpdate?.Invoke(content);
    }

    private string GetClipboardText()
    {
        if (!OpenClipboard(IntPtr.Zero))
            return null;

        try
        {
            var handle = GetClipboardData(CF_UNICODETEXT);
            if (handle == IntPtr.Zero)
                return null;

            var pointer = GlobalLock(handle);
            if (pointer == IntPtr.Zero)
                return null;

            var text = Marshal.PtrToStringUni(pointer);
            GlobalUnlock(handle);

            return text;
        }
        finally
        {
            CloseClipboard();
        }
    }

    #region Win32 API

    private const int WM_DRAWCLIPBOARD = 0x0308;
    private const int WM_CHANGECBCHAIN = 0x030D;
    private const uint CF_UNICODETEXT = 13;

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool OpenClipboard(IntPtr hWndNewOwner);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool CloseClipboard();

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr GetClipboardData(uint uFormat);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GlobalLock(IntPtr hMem);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool GlobalUnlock(IntPtr hMem);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr DefWindowProc(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern uint GetLastError();

    #endregion

    public Action<object>? ClipboardUpdate { get; set; }
}

public static class Win32MessageManager
{
    private static readonly Dictionary<IntPtr, Func<IntPtr, uint, IntPtr, IntPtr, IntPtr>> MessageHandlers = [];

    public static void AddHook(IntPtr hwnd, Func<IntPtr, uint, IntPtr, IntPtr, IntPtr> handler)
    {
        if (!MessageHandlers.ContainsKey(hwnd))
            MessageHandlers.Add(hwnd, handler);
    }

    public static IntPtr ProcessMessage(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam)
    {
        return MessageHandlers.TryGetValue(hwnd, out var handler)
            ? handler(hwnd, msg, wParam, lParam)
            : DefWindowProc(hwnd, msg, wParam, lParam);
    }

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr DefWindowProc(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
}