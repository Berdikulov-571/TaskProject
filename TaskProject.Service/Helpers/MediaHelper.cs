using TaskProject.Domain.Exceptions.File;

namespace TaskProject.Service.Helpers
{
    public class MediaHelper
    {
        public static string MakeFileName(string filename)
        {
            FileInfo fileInfo = new FileInfo(filename);

            string[] ImageExtension = GetFileExtensions();

            if (ImageExtension.Any(x => x.ToLower() == fileInfo.Extension.ToLower()))
            {
                string extension = fileInfo.Extension;
                string name = "FILE_" + Guid.NewGuid() + extension;
                return name;
            }
            throw new FileNotValid();
        }

        public static string[] GetFileExtensions()
        {
            return new string[]
            {
            ".mp4",
            ".mkv",
            ".mov",
            ".m4v",
            ".webm",
            ".avi",
            ".m4a",
            };
        }
    }
}
