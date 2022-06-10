using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using LOBapp.Services;

using Microsoft.UI.Xaml.Controls;

using Parquet.Data.Rows;

using System;
using System.Collections;

using Windows.Storage.Pickers;

using WinRT.Interop;

namespace LOBapp.ViewModels;

public class RowView
{
    private readonly Row _parquetRow;

    public RowView(Row row)
    {
        this._parquetRow = row;
    }

    public object this[int i]
    {
        get
        {
            return _parquetRow[i];
        }
    }
}
public partial class ViewFilePageViewModel : ObservableObject
{

    public ViewFilePageViewModel(Table table, string path)
    {
        Table = table;
        Path = path;
    }
    public Table Table { get; }
    public string Path { get; }
}
