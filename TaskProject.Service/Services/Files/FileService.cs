using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using TaskProject.Service.Abstractions.File;
using TaskProject.Service.Helpers;

namespace TaskProject.Service.Services.Files
{
    public class FileService : IFileService
    {
        private readonly string MEDIA = "media";
        private readonly string VIDEOS = "videos";
        private readonly string ROOTPATH;

        public FileService(IWebHostEnvironment env)
        {
            ROOTPATH = env.WebRootPath;
        }

        public async ValueTask<string> UploadFileAsync(IFormFile file)
        {
            string newImageName = MediaHelper.MakeFileName(file.FileName.ToLower());
            string subPath = Path.Combine(MEDIA, VIDEOS, newImageName);
            string path = Path.Combine(ROOTPATH, subPath);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
                return subPath;
            }
        }


        public async ValueTask<bool> DeletFileAsync(string file)
        {
            string path = Path.Combine(ROOTPATH, file);

            if (File.Exists(path))
            {
                await Task.Run(() =>
                {
                    File.Delete(path);
                });
                return true;
            }
            return false;
        }

        public async ValueTask<byte[]> GetFileAsync(string fileName)
        {
            string path = Path.Combine(ROOTPATH, fileName);
            byte[] imageBytes = await File.ReadAllBytesAsync(path);
            return imageBytes;
        }
    }
}