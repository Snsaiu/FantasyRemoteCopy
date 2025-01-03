﻿using System.Diagnostics.CodeAnalysis;

namespace AirTransfer.Native;

[SuppressMessage("ReSharper", "IdentifierTypo")]
public enum WindowsMessages : uint
{
    /// <summary>
    /// Notifies a window that the user clicked the right mouse button (right-clicked) in the window.
    /// See <a href="https://docs.microsoft.com/en-us/windows/win32/menurc/wm-contextmenu">WM_CONTEXTMENU message</a>
    ///
    /// In case of a notify icon:
    /// If a user selects a notify icon's shortcut menu with the keyboard, the Shell now sends the associated application a WM_CONTEXTMENU message. Earlier versions send WM_RBUTTONDOWN and WM_RBUTTONUP messages.
    /// See <a href="https://docs.microsoft.com/en-us/windows/win32/api/shellapi/nf-shellapi-shell_notifyiconw">Shell_NotifyIcon function</a>
    /// </summary>
    WM_CONTEXTMENU = 0x007b,

    /// <summary>
    /// Posted to a window when the cursor moves.
    /// If the mouse is not captured, the message is posted to the window that contains the cursor.
    /// Otherwise, the message is posted to the window that has captured the mouse.
    ///
    /// See <a href="https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-mousemove">WM_MOUSEMOVE message</a>
    /// </summary>
    WM_MOUSEMOVE = 0x0200,

    /// <summary>
    /// Posted when the user presses the left mouse button while the cursor is in the client area of a window.
    /// If the mouse is not captured, the message is posted to the window beneath the cursor.
    /// Otherwise, the message is posted to the window that has captured the mouse.
    ///
    /// See <a href="https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-lbuttondown">WM_LBUTTONDOWN message</a>
    /// </summary>
    WM_LBUTTONDOWN = 0x0201,

    /// <summary>
    /// Posted when the user releases the left mouse button while the cursor is in the client area of a window.
    /// If the mouse is not captured, the message is posted to the window beneath the cursor.
    /// Otherwise, the message is posted to the window that has captured the mouse.
    ///
    /// See <a href="https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-lbuttonup">WM_LBUTTONUP message</a>
    /// </summary>
    WM_LBUTTONUP = 0x0202,

    /// <summary>
    /// Posted when the user double-clicks the left mouse button while the cursor is in the client area of a window.
    /// If the mouse is not captured, the message is posted to the window beneath the cursor.
    /// Otherwise, the message is posted to the window that has captured the mouse.
    ///
    /// See <a href="https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-lbuttondblclk">WM_LBUTTONDBLCLK message</a>
    /// </summary>
    WM_LBUTTONDBLCLK = 0x0203,

    /// <summary>
    /// Posted when the user presses the right mouse button while the cursor is in the client area of a window.
    /// If the mouse is not captured, the message is posted to the window beneath the cursor.
    /// Otherwise, the message is posted to the window that has captured the mouse.
    ///
    /// See <a href="https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-rbuttondown">WM_RBUTTONDOWN message</a>
    /// </summary>
    WM_RBUTTONDOWN = 0x0204,

    /// <summary>
    /// Posted when the user releases the right mouse button while the cursor is in the client area of a window.
    /// If the mouse is not captured, the message is posted to the window beneath the cursor.
    /// Otherwise, the message is posted to the window that has captured the mouse.
    ///
    /// See <a href="https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-rbuttonup">WM_RBUTTONUP message</a>
    /// </summary>
    WM_RBUTTONUP = 0x0205,

    /// <summary>
    /// Posted when the user double-clicks the right mouse button while the cursor is in the client area of a window.
    /// If the mouse is not captured, the message is posted to the window beneath the cursor.
    /// Otherwise, the message is posted to the window that has captured the mouse.
    ///
    /// See <a href="https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-rbuttondblclk">WM_RBUTTONDBLCLK message</a>
    /// </summary>
    WM_RBUTTONDBLCLK = 0x0206,

    /// <summary>
    /// Posted when the user presses the middle mouse button while the cursor is in the client area of a window.
    /// If the mouse is not captured, the message is posted to the window beneath the cursor.
    /// Otherwise, the message is posted to the window that has captured the mouse.
    ///
    /// See <a href="https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-mbuttondown">WM_MBUTTONDOWN message</a>
    /// </summary>
    WM_MBUTTONDOWN = 0x0207,

    /// <summary>
    /// Posted when the user releases the middle mouse button while the cursor is in the client area of a window.
    /// If the mouse is not captured, the message is posted to the window beneath the cursor.
    /// Otherwise, the message is posted to the window that has captured the mouse.
    ///
    /// See <a href="https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-mbuttonup">WM_MBUTTONUP message</a>
    /// </summary>
    WM_MBUTTONUP = 0x0208,

    /// <summary>
    /// Posted when the user double-clicks the middle mouse button while the cursor is in the client area of a window.
    /// If the mouse is not captured, the message is posted to the window beneath the cursor.
    /// Otherwise, the message is posted to the window that has captured the mouse.
    ///
    /// See <a href="https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-mbuttondblclk">WM_MBUTTONDBLCLK message</a>
    /// </summary>
    WM_MBUTTONDBLCLK = 0x0209,

    /// <summary>
    /// Sent when the effective dots per inch (dpi) for a window has changed.
    /// The DPI is the scale factor for a window.
    /// There are multiple events that can cause the DPI to change.
    /// </summary>
    WM_DPICHANGED = 0x02e0,

    /// <summary>
    /// Used to define private messages for use by private window classes, usually of the form WM_USER+x, where x is an integer value.
    /// </summary>
    WM_USER = 0x0400,

    /// <summary>
    /// This message is only send when using NOTIFYICON_VERSION_4, the Shell now sends the associated application an NIN_SELECT notification.
    /// Send when a notify icon is activated with mouse or ENTER key.
    /// Earlier versions send WM_RBUTTONDOWN and WM_RBUTTONUP messages.
    /// </summary>
    NIN_SELECT = WM_USER,

    /// <summary>
    /// This message is only send when using NOTIFYICON_VERSION_4, the Shell now sends the associated application an NIN_SELECT notification.
    /// Send when a notify icon is activated with SPACEBAR or ENTER key.
    /// Earlier versions send WM_RBUTTONDOWN and WM_RBUTTONUP messages.
    /// </summary>
    NIN_KEYSELECT = WM_USER + 1,

    /// <summary>
    /// Sent when the balloon is shown (balloons are queued).
    /// </summary>
    NIN_BALLOONSHOW = WM_USER + 2,

    /// <summary>
    /// Sent when the balloon disappears. For example, when the icon is deleted.
    /// This message is not sent if the balloon is dismissed because of a timeout or if the user clicks the mouse.
    ///
    /// As of Windows 7, NIN_BALLOONHIDE is also sent when a notification with the NIIF_RESPECT_QUIET_TIME flag set attempts to display during quiet time (a user's first hour on a new computer).
    /// In that case, the balloon is never displayed at all.
    /// </summary>
    NIN_BALLOONHIDE = WM_USER + 3,

    /// <summary>
    /// Sent when the balloon is dismissed because of a timeout.
    /// </summary>
    NIN_BALLOONTIMEOUT = WM_USER + 4,

    /// <summary>
    /// Sent when the balloon is dismissed because the user clicked the mouse.
    /// </summary>
    NIN_BALLOONUSERCLICK = WM_USER + 5,

    /// <summary>
    /// Sent when the user hovers the cursor over an icon to indicate that the richer pop-up UI should be used in place of a standard textual tooltip.
    /// </summary>
    NIN_POPUPOPEN = WM_USER + 6,

    /// <summary>
    /// Sent when a cursor no longer hovers over an icon to indicate that the rich pop-up UI should be closed.
    /// </summary>
    NIN_POPUPCLOSE = WM_USER + 7
}