using Infrastructure.ExternalServices.Storage.Local;

namespace Infrastructure.ExternalServices.Storage;
public class StorageOptions
{
    public string Provider { get; set; }
    public string FolderPath { get; set; }
    public LocalOptions Local { get; set; }
}
