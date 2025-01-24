using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ECommerceApi.Application.Abstractions.Storage.Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ECommerceApi.Infrastructure.Services.Storage.Azure;

public class AzureStorage : Storage, IAzureStorage
{
    private readonly BlobServiceClient _blobServiceClient; // used to connect to azure storage account
    BlobContainerClient _blobContainerClient; // file operations on the target container in the account
    // alseo enable the “Blob anonymous access” setting in azure
    
    public AzureStorage(IConfiguration configuration)
    {
        _blobServiceClient = new(configuration["Storage:Azure"]);
    }

    public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string containerName, IFormFileCollection files)
    {
        _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        await _blobContainerClient.CreateIfNotExistsAsync();
        await _blobContainerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);
        
        List<(string fileName, string pathOrContainerName)> datas = new();
        foreach (IFormFile file in files)
        {
            string fileNewName = await RenameFileAsync(containerName, file.Name, HasFile);
            
            BlobClient blobClient = _blobContainerClient.GetBlobClient(fileNewName);
            await blobClient.UploadAsync(file.OpenReadStream());
            datas.Add((fileNewName, containerName));
        }
        return datas;
    }

    public async Task DeleteAsync(string containerName, string fileName)
    {
        _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
        await blobClient.DeleteAsync();
    }

    public List<string> GetFiles(string containerName)
    {
        _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        return _blobContainerClient.GetBlobs().Select(blob => blob.Name).ToList();
    }

    public bool HasFile(string containerName, string fileName)
    {
        _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        return _blobContainerClient.GetBlobs().Any(blob => blob.Name == fileName);
    }
}