using Application.Common.Interfaces;
using Application.Common.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Constants;
using System.Net.Http.Headers;

namespace Infrastructure.ExternalServices;

public class LocalStorageService : IFileService
{
    private readonly FileStorageSettings _fileStorage;
    private readonly string _userContentFolder;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ILogger<LocalStorageService> _logger;
    private readonly string _baseUrl;

    public LocalStorageService(
       IWebHostEnvironment webHostEnvironment,
       ILogger<LocalStorageService> logger,
       AppSettings appSettings)
    {
        _webHostEnvironment = webHostEnvironment;
        _logger = logger;
        _fileStorage = appSettings.FileStorageSettings;
        //_userContentFolder = Path.Combine(_webHostEnvironment.ContentRootPath, _fileStorage.Path);
        _userContentFolder = Path.Combine(_webHostEnvironment.WebRootPath, _fileStorage.Path); // wwwroot
        _baseUrl = appSettings.BaseURL;

    }

    public async Task<FileUploadResult> AddFileAsync(IFormFile file)
    {
        Stream mediaBinaryStream = file.OpenReadStream();

        var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";


        var filePath = Path.Combine(_userContentFolder, fileName);
        // Ensure the directory exists
        var directory = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        try
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await mediaBinaryStream.CopyToAsync(fileStream);
            }
            _logger.LogInformation($"File {fileName} saved successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error saving file {fileName}");
            throw new IOException($"An error occurred while saving the file {fileName}.", ex);
        }

        //var publicUrl = $"{_baseUrl.TrimEnd('/')}/{_fileStorage.Path}/{fileName}";

        return new FileUploadResult
        {
            Name = fileName,
            //Path = GetPublicUrl(fileName)
            Path = _fileStorage.Path,
            BaseUrl = _baseUrl
        };
    }

    public Task DeleteFileAsync(DeleteFileRequest request)
    {
        var relativePath = request.FileName.Replace(_baseUrl, "").TrimStart('/');
        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, relativePath);
        //var filePath = Path.Combine(_userContentFolder, request.FileName);
        if (File.Exists(filePath))
        {
            try
            {
                File.Delete(filePath);
                _logger.LogInformation($"File {request.FileName} deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting file {request.FileName}");
                throw new IOException($"An error occurred while deleting the file {request.FileName}.", ex);
            }
        }
        else
        {
            _logger.LogWarning($"File {request.FileName} not found.");
        }
        return Task.CompletedTask;
    }

    // Convert physical url to public url
    private string GetPublicUrl(string fileName) => $"/{_fileStorage.Path}/{fileName}";
    //private string GetPublicUrl(string fileName) => $"{_baseUrl.TrimEnd('/')}/{_fileStorage.Path}/{fileName}";

    public string GetFileUrl(string fileName)
    {
        var filePath = Path.Combine(_userContentFolder, fileName);
        if (File.Exists(filePath))
        {
            try
            {
                var fileUrl = Path.Combine(_baseUrl, _userContentFolder, fileName);
                _logger.LogInformation($"File URL for {fileName} retrieved successfully.");
                return fileUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving file URL for {fileName}");
                throw new InvalidOperationException($"An error occurred while retrieving the file URL for {fileName}.", ex);
            }
        }
        else
        {
            _logger.LogWarning($"File {fileName} not found.");
            throw new InvalidOperationException($"File {fileName} not found.");
        }
    }
}
