using LOBapp.Services;

using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;

using System;

using Windows.Foundation;

namespace LOBapp;
/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{
    private AppWindow appWindow;
    private Window m_window;
    public Window Window => m_window;
    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        this.InitializeComponent();
    }

    /// <summary>
    /// Invoked when the application is launched normally by the end user.  Other entry points
    /// will be used such as when the application is launched to open a specific file.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {   
        m_window = new MainWindow();
        m_window.Activate();
        if (AppWindowTitleBar.IsCustomizationSupported()) //Run only on Windows 11
        {
            m_window.SizeChanged += App.SizeChanged(m_window); //Register handler for setting draggable rects
            appWindow = App.GetAppWindow(m_window); //Set ExtendsContentIntoTitleBar for the AppWindow not the window
            appWindow.TitleBar.ExtendsContentIntoTitleBar = true;
            appWindow.TitleBar.ButtonBackgroundColor = Colors.Transparent;
            appWindow.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        }
        BackDrop backdrop = new(m_window);
        backdrop.SetBackdrop();
    }
    public static TypedEventHandler<object, WindowSizeChangedEventArgs> SizeChanged(Window window)
    {
        return (sender, args) =>
        {
            //Update the title bar draggable region. We need to indent from the left both for the nav back button and to avoid the system menu
            Windows.Graphics.RectInt32[] rects = new Windows.Graphics.RectInt32[] { new Windows.Graphics.RectInt32(48, 0, (int)args.Size.Width - 48, 48) };
            GetAppWindow(window).TitleBar.SetDragRectangles(rects);
        };
    }
    public static AppWindow GetAppWindow(Window window)
    {
        IntPtr hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
        WindowId windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
        return AppWindow.GetFromWindowId(windowId);
    }

}
