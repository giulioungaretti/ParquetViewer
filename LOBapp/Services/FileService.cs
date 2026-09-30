using Parquet;
using Parquet.Rows;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LOBapp.Services;

public static class FileService
{
    public static async Task<Table> LoadAsync(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        if (!string.Equals(Path.GetExtension(path), ".parquet", StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException("Only .parquet files are supported.", nameof(path));
        }

        await using FileStream stream = new(path, FileMode.Open, FileAccess.Read, FileShare.Read,
            bufferSize: 4096, options: FileOptions.Asynchronous | FileOptions.RandomAccess);
        var options = new ParquetOptions { TreatByteArrayAsString = true };
        using var reader = await ParquetReader.CreateAsync(stream, options).ConfigureAwait(false);
        return await reader.ReadAsTableAsync().ConfigureAwait(false);
    }
}
