using System.Data;
using System.Text.Json;
using System.Text;
using ExcelDataReader;

namespace Blazorcrud.Server.Services;

public interface IFileRepository
{
    DataTable? ReadExcelFile(string path);

    Task<T?> ReadFile<T>(string filePath);
}

public class FileRepository : IFileRepository
{
    private static readonly object _lock = new object();
    public DataTable? ReadExcelFile(string filePath)
    {
        if (!File.Exists(filePath)){
            return null; //maybe throw new FileNotFoundException($"File not exits in the folder {filePath}") ?????
        }

        //concurrency lock on reading file
        lock(_lock)
        {
            using(var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    
                var configReader = new ExcelReaderConfiguration() {
                    FallbackEncoding = Encoding.GetEncoding(1252)
                };

                using (var reader = ExcelReaderFactory.CreateReader(stream, configReader))
                {
                    var config = new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = (s) => new ExcelDataTableConfiguration { UseHeaderRow = true }
                    };

                    return reader.AsDataSet(config).Tables[0];
                }
            }
        }
    }

    public async Task<T?> ReadFile<T>(string filePath)
    {
        await using FileStream stream = File.OpenRead(filePath);
        return await JsonSerializer.DeserializeAsync<T>(stream);
    }
}


