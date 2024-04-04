using Microsoft.AspNetCore.Http;

namespace TaskProject.Service.Abstractions.File
{
    public interface IFileService
    {
        ValueTask<string> UploadFileAsync(IFormFile file);

        ValueTask<bool> DeletFileAsync(string file);

        ValueTask<byte[]> GetFileAsync(string path);
    }
}