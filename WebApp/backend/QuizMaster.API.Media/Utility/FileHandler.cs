using QuizMaster.API.Media.Models;

namespace QuizMaster.API.Media.Utility
{
    public class FileHandler
    {
        public static async Task<FileInformation?> SaveFile(IWebHostEnvironment hostEnvironment,IFormFile formFile)
        {
            FileInformation fileInformation = new();

            // generate a Guid for the FileInformation
            fileInformation.Id = Guid.NewGuid();

            // check if file name is null, if yes, return null
            if (formFile.FileName == null) return null;

            // check if filename length is 0, return null
            if (formFile.FileName.Length == 0) return null;

            // get the file type (extension)
            fileInformation.Type = Path.GetExtension(formFile.FileName);
            fileInformation.Name = Path.GetFileNameWithoutExtension(formFile.FileName);

            // convert the file size to MB
            long fileSizeInByte = formFile.Length;
            double fileSizeInKb = fileSizeInByte / 1024;
            double fileSizeinMb = fileSizeInKb / 1024;

            // save the file size in MB(MegaByte)
            fileInformation.Size = fileSizeinMb;


            // check if Image directory exists, otherwise create a new directory
            bool imageDir = Directory.Exists(Path.Combine(hostEnvironment.WebRootPath, "Images/"));
            if(!imageDir) Directory.CreateDirectory(Path.Combine(hostEnvironment.WebRootPath, "Images/"));

            // save path (wwwroot/Images/{guid}.{ext})
            string savePath = Path.Combine(hostEnvironment.WebRootPath, "Images/", $"{fileInformation.Id}{fileInformation.Type}");

            try
            {
                // copy the file using file stream (upload)
                using FileStream stream = new(savePath, FileMode.CreateNew);
                await formFile.CopyToAsync(stream);
                stream.Close();
            }
            catch { return null; }

            return fileInformation;
        }

        public static bool DeleteFile(IWebHostEnvironment hostEnvironment, FileInformation fileInformation)
        {
            string filePath = Path.Combine(hostEnvironment.WebRootPath, "Images", $"{fileInformation.Id}{fileInformation.Type}");

            // check if file exists, if exists, delete
            try
            {
                bool fileExists = File.Exists(filePath);
                if (fileExists)
                {
                    File.Delete(filePath);
                }
                else { return false; }
            }
            catch { return false; }
            return true;
        }

    }
}
