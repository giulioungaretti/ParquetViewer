using Parquet.Rows;

namespace LOBapp.ViewModels;

public sealed class ViewFilePageViewModel
{
    public ViewFilePageViewModel(Table table, string path)
    {
        Table = table;
        Path = path;
    }
    public Table Table { get; }
    public string Path { get; }
}
