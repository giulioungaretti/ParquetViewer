using CommunityToolkit.WinUI.UI.Controls;

using LOBapp.ViewModels;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;

using Parquet.Data;

namespace LOBapp;

public sealed partial class ViewFilePage : Page
{
    public ViewFilePageViewModel ViewModel { get; }

    public ViewFilePage(ViewFilePageViewModel vm)
    {
        this.InitializeComponent();
        this.ViewModel = vm;

        AddColumnsHeader();
    }

    // Add columns from schema
    // TODO: we have the parquet types could be fun to use them for the coulmns
    private void AddColumnsHeader()
    {
        int i = 0;
        foreach (Field f in ViewModel.Table.Schema.Fields)
        {
            ParquetDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = f.Name,
                Width = DataGridLength.SizeToCells,
                Binding = new Binding
                {
                    Path = new PropertyPath("[" + i++ + "]")
                }
            });

        }
    }

    private void ParquetDataGrid_Loaded(object sender, RoutedEventArgs e)
    {


    }
}

