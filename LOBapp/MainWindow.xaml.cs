using LOBapp.Services;
using LOBapp.ViewModels;

using Microsoft.UI.Xaml;

namespace LOBapp;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        this.InitializeComponent();
        ViewModel = new MainPageViewModel(this);

        BackDrop backdrop = new(this);
        backdrop.SetBackdrop();

        // TODO: style title bar
        //ExtendsContentIntoTitleBar = true;
        //SetTitleBar(AppTitleBar);
    }

    public MainPageViewModel ViewModel { get;}
    public void SetBody(UIElement uiElement) => Body.Content = uiElement;

    public UIElement GetBody() => Body;
}
