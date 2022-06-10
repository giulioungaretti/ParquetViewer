using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using LOBapp.Services;

using Microsoft.UI.Xaml.Controls;

using System;

using Windows.Storage.Pickers;

using WinRT.Interop;

namespace LOBapp.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    readonly IntPtr hwnd;
    readonly MainWindow mainWindow;

    public MainPageViewModel(MainWindow window)
    {
        mainWindow = window;
        hwnd = WindowNative.GetWindowHandle(window);
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Errored))]
    string error;
    public bool Errored => Error != null;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotDoingIO))]
    bool isDoingIO;
    public bool IsNotDoingIO => !IsNotDoingIO;

    [ObservableProperty]
    private string infoBarMsg;

    [ObservableProperty]
    private InfoBarSeverity infoBarSev;

    [ObservableProperty]
    private bool infoBarOpen;

    [ObservableProperty]
    private string filePath;

    public void SetErorr(string msg)
    {
        InfoBarMsg = msg;
        InfoBarOpen = true;
        InfoBarSev = InfoBarSeverity.Error;
    }
    public void SetInfo(string msg)
    {
        InfoBarMsg = msg;
        InfoBarOpen = true;
        InfoBarSev = InfoBarSeverity.Informational;
    }

    public void CloseMsgBar()
    {
        InfoBarMsg = null;
        InfoBarOpen = false;
    }
    [RelayCommand]
    public void Close()
    {
        filePath = null;
        CloseMsgBar();
        mainWindow.SetBody(new Frame());
    }

    [RelayCommand]
    public async void GetTableAsync()
    {
        if (IsDoingIO)
        {
            return;
        }
        try
        {
            IsDoingIO = true;

            FileOpenPicker filePicker = new();
            filePicker.FileTypeFilter.Add(".parquet");
            InitializeWithWindow.Initialize(filePicker, hwnd);
            var file = await filePicker.PickSingleFileAsync();

            var result = await FileService.LoadAsync(file);
            if (result.Error != null)
            {
                error = result.Error;
                SetErorr(error);
                return;
            }

            SetInfo($"Loaded {file.Path}");
            FilePath = file.Path;
            IsDoingIO = false;

            ViewFilePageViewModel vm = new(result.Data, result.Name);
            mainWindow.SetBody(new ViewFilePage(vm));

        }
        catch (Exception ex)
        {
            error = $"Unhanlded error{ex.Message}";
            SetErorr(error);
        }
        finally
        {
            isDoingIO = false;
        }
    }
}
