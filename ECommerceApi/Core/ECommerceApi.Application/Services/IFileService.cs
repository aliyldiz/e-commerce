using Microsoft.AspNetCore.Http;

namespace ECommerceApi.Application.Services;

public interface IFileService
{
    Task<List<(string fileName, string path)>> UploadFileAsync(string path, IFormFileCollection files);
    Task<bool> CopyFileAsync(string path, IFormFile file);
}