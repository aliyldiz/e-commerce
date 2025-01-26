using ECommerceApi.Application.Abstractions.Storage.Local;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ECommerceApi.Infrastructure.Services.Storage.Local;

public class LocalStorage : Storage, ILocalStorage
{
    private readonly IWebHostEnvironment _webHostEnvironment;
        
    public LocalStorage(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }
    
    public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string path, IFormFileCollection files)
    {
        string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, path);
        List<(string filename, string path)> datas = new();
        
        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);

        foreach (IFormFile file in files)
        {
            string fileNewName = await RenameFileAsync(uploadPath, file.Name, HasFile);
            
            await CopyFileAsync($"{uploadPath}/{fileNewName}", file);
            datas.Add((fileNewName, $"{path}/{fileNewName}"));
        }
        
        // TODO: yukarıdaki if geçerli değilse burada dosyaların sunucuda yüklenirken hata alındığına dair uyarıcı bir exception oluşturup firlatılması gerekiyor. 
        return datas;
    }
    
    async Task CopyFileAsync(string path, IFormFile file)
    {
        try
        {
            await using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024*1024, false);
        
            await file.CopyToAsync(fileStream);
            await fileStream.FlushAsync();
        }
        catch (Exception e)
        {
            // TODO: log
            Console.WriteLine(e);
            throw;
        }
    }

    public Task DeleteAsync(string path, string fileName)
    {
        File.Delete($"{path}/{fileName}");
        return Task.CompletedTask;
    }

    public List<string> GetFiles(string path)
    {
        DirectoryInfo dir = new(path);
        return dir.GetFiles().Select(f => f.Name).ToList();
    }

    public new bool HasFile(string path, string fileName)
        => File.Exists($"{path}/{fileName}");
}