using Parquet;
using Parquet.Data.Rows;

using System;
using System.IO;
using System.Threading.Tasks;

using Windows.Storage;
using Windows.Storage.Streams;

namespace LOBapp.Services;


public sealed class TableResult
{
    public TableResult(string name, Table data, string error)
    {
        Name = name;
        Data = data;
        Error = error;
    }

    public string Name { get; set; }
    public Table Data { get; set; }
    public string Error { get; set; }
}

public class FileService
{
    public static async Task<TableResult> LoadAsync(StorageFile file)
    {
        if (file != null)
        {
            if (file.Name.EndsWith("parquet", StringComparison.OrdinalIgnoreCase))
            {
                using IRandomAccessStreamWithContentType stream = await file.OpenReadAsync();
                using Stream systemStream = stream.AsStreamForRead();
                var formatOptions = new ParquetOptions
                {
                    // TODO: read docs
                    TreatByteArrayAsString = true
                };
                using var reader = await ParquetReader.CreateAsync(systemStream, formatOptions);
                var table = await reader.ReadAsTableAsync();
                return new TableResult(file.Name, table, null);
            }
            return new TableResult(null, null, "Only .parquet files are supported");
        }
        return new TableResult(null, null, "No files selected");
    }
}
