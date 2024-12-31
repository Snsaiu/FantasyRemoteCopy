using System.Runtime.InteropServices;
using AirTransfer.Interfaces;
using Foundation;
using ObjCRuntime;
using AppKit;

namespace AirTransfer;

public sealed class TrayService : NSObject, ITrayService
{
    [DllImport("/usr/lib/libobjc.dylib", EntryPoint = "objc_msgSend")]
    public static extern IntPtr IntPtr_objc_msgSend_nfloat(IntPtr receiver, IntPtr selector, nfloat arg1);

    [DllImport("/usr/lib/libobjc.dylib", EntryPoint = "objc_msgSend")]
    public static extern IntPtr IntPtr_objc_msgSend_IntPtr(IntPtr receiver, IntPtr selector, IntPtr arg1);

    [DllImport("/usr/lib/libobjc.dylib", EntryPoint = "objc_msgSend")]
    public static extern IntPtr IntPtr_objc_msgSend(IntPtr receiver, IntPtr selector);

    [DllImport("/usr/lib/libobjc.dylib", EntryPoint = "objc_msgSend")]
    public static extern void void_objc_msgSend_IntPtr(IntPtr receiver, IntPtr selector, IntPtr arg1);

    [DllImport("/usr/lib/libobjc.dylib", EntryPoint = "objc_msgSend")]
    public static extern void void_objc_msgSend_bool(IntPtr receiver, IntPtr selector, bool arg1);

    NSObject systemStatusBarObj;
    NSObject statusBarObj;
    NSObject statusBarItem;
    NSObject statusBarButton;
    NSObject statusBarImage;

    public Action ClickHandler { get; set; }

    public void Initialize()
    {
        statusBarObj = Runtime.GetNSObject(Class.GetHandle("NSStatusBar"));
        systemStatusBarObj = statusBarObj.PerformSelector(new Selector("systemStatusBar"));
        statusBarItem = Runtime.GetNSObject(IntPtr_objc_msgSend_nfloat(systemStatusBarObj.Handle, Selector.GetHandle("statusItemWithLength:"), -1));
        statusBarButton = Runtime.GetNSObject(IntPtr_objc_msgSend(statusBarItem.Handle, Selector.GetHandle("button")));
        statusBarImage = Runtime.GetNSObject(IntPtr_objc_msgSend(ObjCRuntime.Class.GetHandle("NSImage"), Selector.GetHandle("alloc")));

        var imgPath = System.IO.Path.Combine(NSBundle.MainBundle.BundlePath, "Contents", "Resources", "Platforms", "MacCatalyst", "trayicon.png");
        if (!System.IO.File.Exists(imgPath))
        {
            Console.WriteLine("Image file not found: " + imgPath);
            return;
        }

        var imageFileStr = NSString.CreateNative(imgPath);
        var nsImagePtr = IntPtr_objc_msgSend_IntPtr(statusBarImage.Handle, Selector.GetHandle("initWithContentsOfFile:"), imageFileStr);

        void_objc_msgSend_IntPtr(statusBarButton.Handle, Selector.GetHandle("setImage:"), statusBarImage.Handle);
        void_objc_msgSend_bool(nsImagePtr, Selector.GetHandle("setTemplate:"), true);

        // Handle click
        void_objc_msgSend_IntPtr(statusBarButton.Handle, Selector.GetHandle("setTarget:"), this.Handle);
        void_objc_msgSend_IntPtr(statusBarButton.Handle, Selector.GetHandle("setAction:"), new Selector("handleButtonClick:").Handle);
        const IntPtr leftMouseUp = 1 << 1;
        const IntPtr rightMouseUp = 1 << 3;
        void_objc_msgSend_IntPtr(statusBarButton.Handle, Selector.GetHandle("sendActionOn:"), leftMouseUp | rightMouseUp);
    }

    
    [Export("handleButtonClick:")]
    void HandleClick(NSObject senderStatusBarButton)
    {
        var nsapp = Runtime.GetNSObject(Class.GetHandle("NSApplication"));
        var sharedApp = nsapp.PerformSelector(new Selector("sharedApplication"));

        // Activate the app and bring it to the front
        void_objc_msgSend_bool(sharedApp.Handle, Selector.GetHandle("activateIgnoringOtherApps:"), true);
      
        var currentEvent = Runtime.GetNSObject(IntPtr_objc_msgSend(sharedApp.Handle, Selector.GetHandle("currentEvent")));
        var eventType = IntPtr_objc_msgSend(currentEvent.Handle, Selector.GetHandle("type"));
        if (eventType == 1)
        {
            ClickHandler?.Invoke();
        }
        else
        {
            // 右键点击
            
        }
     
        
      //  ClickHandler?.Invoke();
    }
    
    public void ShowContextMenu()
    {
        
    }
    
    
}