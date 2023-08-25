using LOBapp.ViewModels;

using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Windows.Foundation;

namespace LOBapp;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        this.InitializeComponent();
        ViewModel = new MainPageViewModel(this);

        if (AppWindowTitleBar.IsCustomizationSupported()) //Run only on Windows 11
        {
            AppWindow.TitleBar.ExtendsContentIntoTitleBar = true;
            AppWindow.TitleBar.ButtonBackgroundColor = Colors.Transparent;
            AppWindow.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        }
        SystemBackdrop = new MicaBackdrop();
    }

    public MainPageViewModel ViewModel { get; }
    public void SetBody(UIElement uiElement) => Body.Content = uiElement;

    public UIElement GetBody() => Body;


    public string GetAppTitleFromSystem()
    {
        var title = Windows.ApplicationModel.Package.Current.DisplayName;
        if (!string.IsNullOrEmpty(title))
        {
            return "thte title";
        }
        return Windows.ApplicationModel.Package.Current.DisplayName;
    }

    public static new TypedEventHandler<object, WindowSizeChangedEventArgs> SizeChanged(Window window)
    {
        return (sender, args) =>
        {
            //Update the title bar draggable region. We need to indent from the left both for the nav back button and to avoid the system menu
            Windows.Graphics.RectInt32[] rects = new Windows.Graphics.RectInt32[] { new Windows.Graphics.RectInt32(48, 0, (int)args.Size.Width - 48, 48) };
            window.AppWindow.TitleBar.SetDragRectangles(rects);
        };
    }
}
