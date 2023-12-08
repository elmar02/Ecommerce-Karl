namespace BizNews.Helper
{
    public static class FileHelper
    {
        //public static async Task<string> SaveFileAsync(this IFormFile file, string WebRootPath, string folderPath)
        //{
        //    foreach (var p in folderPath.Split('/'))
        //    {
        //        var filePath = Path.Combine(WebRootPath, p).ToLower();
        //        if (!Directory.Exists(filePath))
        //        {
        //            Directory.CreateDirectory(filePath);
        //        }
        //    }
        //    var path = folderPath + Guid.NewGuid().ToString() + file.FileName;
        //    using FileStream fileStream = new(WebRootPath + path, FileMode.Create);
        //    await file.CopyToAsync(fileStream);
        //    return path;
        //}
    }
}
