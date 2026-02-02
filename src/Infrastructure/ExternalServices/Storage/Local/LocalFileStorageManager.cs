namespace Infrastructure.ExternalServices.Storage.Local;

public class LocalFileStorageManager : IFileStorageManager
{
    private readonly LocalOptions _options;

    public LocalFileStorageManager(LocalOptions options)
    {
        _options = options;
    }

    public async Task CreateAsync(IFileEntry fileEntry, Stream stream, CancellationToken cancellationToken = default)
    {
        var filePath = Path.Combine(_options.Path, fileEntry.FileLocation);

        var folder = Path.GetDirectoryName(filePath);

        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }   

        using(var fileStream = File.Create(filePath))
        {
            await stream.CopyToAsync(fileStream, cancellationToken);
        }
    }

    public async Task DeleteAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default)
    {
        await Task.Run(() =>
        {
            var filePath = Path.Combine(_options.Path, fileEntry.FileLocation);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }, cancellationToken);
    }

    public Task<byte[]> ReadAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default)
    {
        return File.ReadAllBytesAsync(Path.Combine(_options.Path, fileEntry.FileLocation), cancellationToken);
    }
}
