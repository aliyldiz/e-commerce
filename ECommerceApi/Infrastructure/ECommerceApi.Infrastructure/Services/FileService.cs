using ECommerceApi.Application.Services;
using ECommerceApi.Infrastructure.Operations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ECommerceApi.Infrastructure.Services;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _webHostEnvironment;
        
    public FileService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }
    
    public async Task<List<(string fileName, string path)>> UploadFileAsync(string path, IFormFileCollection files)
    {
        string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, path);
        List<bool> results = new();
        List<(string filename, string path)> datas = new();
        
        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);

        foreach (IFormFile file in files)
        {
            string fileNewName = await RenameFileAsync(uploadPath, file.FileName);
            
            bool result = await CopyFileAsync($"{uploadPath}/{fileNewName}", file);
            datas.Add((fileNewName, $"{uploadPath}/{fileNewName}"));
            results.Add(result);
        }

        if (results.TrueForAll(r => r.Equals(true)))
            return datas;
        
        // TODO: yukarıdaki if geçerli değilse burada dosyaların sunucuda yüklenirken hata alındığına dair uyarıcı bir exception oluşturup firlatılması gerekiyor. 
        return null;
    }

    async Task<string> RenameFileAsync(string path, string fileName, bool first = true)
    {
        string newFileName = await Task.Run<string>(async () =>
        {
            string extension = Path.GetExtension(fileName);
            string newFileName = string.Empty;
            
            if (first)
            {
                string oldName = Path.GetFileNameWithoutExtension(fileName);
                newFileName = $"{ NameOperation.CharacterRegulatory(oldName) }{extension}";
            }
            else
            {
                newFileName = fileName;
                int index = newFileName.IndexOf("-");
                if (index == -1)
                    newFileName = $"{Path.GetFileNameWithoutExtension(newFileName)}-2{extension}";
                else
                {
                    int lastIndex = 0;
                    while (true)
                    {
                        lastIndex = index;
                        index = newFileName.IndexOf("-", index + 1);
                        if (index == -1)
                        {
                            index = lastIndex;
                            break;
                        }
                    }
                    int index2 = newFileName.IndexOf(".");
                    string fileNo = newFileName.Substring(index + 1, index2 - index - 1);
                    if (int.TryParse(fileNo, out int _fileNo))
                    {
                        _fileNo++;
                        newFileName = newFileName.Remove(index + 1, index2 - index - 1)
                            .Insert(index + 1, _fileNo.ToString());
                    }
                    else
                        newFileName = $"{Path.GetFileNameWithoutExtension(newFileName)}-2{extension}";
                }
            }
            if (File.Exists($"{path}/{newFileName}"))
                return await RenameFileAsync(path, newFileName, false);
            else
                return newFileName;
        });
        return newFileName;
    }

    public async Task<bool> CopyFileAsync(string path, IFormFile file)
    {
        try
        {
            await using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024*1024, false);
        
            await file.CopyToAsync(fileStream);
            await fileStream.FlushAsync();
            return true;
        }
        catch (Exception e)
        {
            // TODO: log
            Console.WriteLine(e);
            throw e;
        }
    }
}