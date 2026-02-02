using Microsoft.AspNetCore.Http;

namespace Application.Common.Interfaces;

public interface IFileService
{
    Task DeleteFileAsync(DeleteFileRequest request);
    Task<UploadFileResult> AddFileAsync(IFormFile file);
    string GetFileUrl(string fileName);
}

public class DeleteFileRequest
{
    public string FileName { get; set; }
}


public class UploadFileResult
{
    public string Name { get; set; } = default!;
    public string Path { get; set; } = default!;
    public string BaseUrl { get; set; } = default!;
}
