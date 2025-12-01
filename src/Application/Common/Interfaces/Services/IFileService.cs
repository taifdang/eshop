using Application.Catalog.Products.Commands.DeleteFile;
using Application.Catalog.Products.Commands.UploadFile;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Interfaces.Services;

public interface IFileService
{
    Task DeleteFileAsync(DeleteFileRequest request);
    Task<FileUploadResult> AddFileAsync(IFormFile file);
    string GetFileUrl(string fileName);
}
