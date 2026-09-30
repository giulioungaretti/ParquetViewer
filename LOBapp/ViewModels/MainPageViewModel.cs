using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using LOBapp.Services;

using Microsoft.UI.Xaml.Controls;
using Parquet;

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using Microsoft.Windows.Storage.Pickers;

namespace LOBapp.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    private readonly MainWindow mainWindow;

    public MainPageViewModel(MainWindow window)
    {
        mainWindow = window;
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotDoingIO))]
    [NotifyCanExecuteChangedFor(nameof(OpenFileCommand), nameof(CloseCommand))]
    public partial bool IsDoingIO { get; set; }

    public bool IsNotDoingIO => !IsDoingIO;

    [ObservableProperty]
    public partial string InfoBarMsg { get; set; } = string.Empty;

    [ObservableProperty]
    public partial InfoBarSeverity InfoBarSev { get; set; }

    [ObservableProperty]
    public partial bool InfoBarOpen { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(WindowTitle))]
    [NotifyCanExecuteChangedFor(nameof(CloseCommand))]
    public partial string FilePath { get; set; } = string.Empty;

    public string WindowTitle => string.IsNullOrEmpty(FilePath) ? "Parquet Viewer" : $"{FilePath} - Parquet Viewer";

    private bool CanOpenFile() => IsNotDoingIO;

    private bool CanClose() => IsNotDoingIO && !string.IsNullOrEmpty(FilePath);

    private void ShowMessage(string message, InfoBarSeverity severity)
    {
        InfoBarMsg = message;
        InfoBarSev = severity;
        InfoBarOpen = true;
    }

    private void CloseMsgBar()
    {
        InfoBarMsg = string.Empty;
        InfoBarOpen = false;
    }

    [RelayCommand(CanExecute = nameof(CanClose))]
    public void Close()
    {
        FilePath = string.Empty;
        CloseMsgBar();
        mainWindow.SetBody(null);
    }

    [RelayCommand(CanExecute = nameof(CanOpenFile))]
    public async Task OpenFileAsync()
    {
        try
        {
            IsDoingIO = true;

            FileOpenPicker filePicker = new(mainWindow.AppWindow.Id);
            filePicker.FileTypeFilter.Add(".parquet");
            var file = await filePicker.PickSingleFileAsync();
            if (file is null)
            {
                return;
            }

            CloseMsgBar();
            string path = file.Path;
            // Parquet decoding is CPU-intensive, even when its I/O APIs are asynchronous.
            var table = await Task.Run(() => FileService.LoadAsync(path));
            ViewFilePageViewModel vm = new(table, path);
            mainWindow.SetBody(new ViewFilePage(vm));
            FilePath = path;
            ShowMessage($"Loaded {path} ({table.Count:N0} rows)", InfoBarSeverity.Informational);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or ArgumentException
            or NotSupportedException or FormatException or COMException or ParquetException)
        {
            ShowMessage($"Could not open the file: {ex.Message}", InfoBarSeverity.Error);
        }
        finally
        {
            IsDoingIO = false;
        }
    }
}
