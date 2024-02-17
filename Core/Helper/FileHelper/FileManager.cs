using Core.Configuration.Concrete;
using Microsoft.AspNetCore.Http;

namespace Core.Helper.FileHelper
{
    public class FileManager : IFileService
    {
        private readonly string WebRootPath = WWWRootConfiguration.WebRootPath;
        public async Task<List<string>> SaveAllFilesAsync(List<IFormFile> files, string folderPath)
        {
            var urls = new List<string>();
            foreach (var file in files)
            {
                var url = await SaveFileAsync(file, folderPath);
                urls.Add(url);
            }
            return urls;
        }

        public async Task<string> SaveFileAsync(IFormFile file, string folderPath)
        {
            var paths = folderPath.Split('/').Where(x=>!string.IsNullOrEmpty(x));
            var route = string.Empty;
            foreach (var p in paths)
            {
                route += p + "\\";
                var filePath = Path.Combine(WebRootPath, route);
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
            }
            var url = folderPath + Guid.NewGuid().ToString() + file.FileName;
            using FileStream fileStream = new(WebRootPath + url, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return url;
        }
    }
}
