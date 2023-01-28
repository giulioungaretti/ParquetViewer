using LOBapp.Services;
using LOBapp.ViewModels;

using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;

namespace LOBapp;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        this.InitializeComponent();
        ViewModel = new MainPageViewModel(this);
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
}
