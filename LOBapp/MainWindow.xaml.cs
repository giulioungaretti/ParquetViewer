using Microsoft.UI.Xaml;

using System;
using WinRT.Interop;

using Windows.Storage.Pickers;

namespace LOBapp
{


    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {

        public MainWindow()
        {
            this.InitializeComponent();
            BackDrop backdrop  = new(this);
            backdrop.SetBackdrop();
            Title = "Open file";
            
        }

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            myButton.Content = "Clicked";
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            var filePicker = new FileOpenPicker();
            filePicker.FileTypeFilter.Add("*");

            var hwnd = WindowNative.GetWindowHandle(this);
            InitializeWithWindow.Initialize(filePicker, hwnd);

            var file = await filePicker.PickSingleFileAsync();    
            if (file != null)
            {
                Title = file.Path;
                InfoBar.Title = "Loaded file";
                InfoBar.Message = file.Path;
                InfoBar.IsOpen = true;
            }

        }
    }
}
