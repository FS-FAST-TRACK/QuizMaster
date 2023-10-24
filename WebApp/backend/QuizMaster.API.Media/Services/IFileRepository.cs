using QuizMaster.API.Media.Models;

namespace QuizMaster.API.Media.Services
{
    public interface IFileRepository
    {
        bool Save(FileInformation fileInfo);
        IEnumerable<FileInformation> GetAll();
        FileInformation? GetFile(Guid Id);
        bool Remove(Guid Id);
    }
}
