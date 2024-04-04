using static TaskProject.Domain.Exceptions.GlobalException;

namespace TaskProject.Domain.Exceptions.File
{
    public class FileNotValid : GlobalException
    {
        public FileNotValid()
        {
            TitleMessage = "File Not Valid!";
        }
    }
}