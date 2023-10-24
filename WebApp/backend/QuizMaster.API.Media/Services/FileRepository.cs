using QuizMaster.API.Media.Data.Context;
using QuizMaster.API.Media.Models;

namespace QuizMaster.API.Media.Services
{
    public class FileRepository : IFileRepository
    {

        private readonly FileDbContext context;
        
        // DI dbcontext in the ctor
        public FileRepository(FileDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<FileInformation> GetAll()
        {
            return context.Files.ToList();
        }

        public FileInformation? GetFile(Guid Id)
        {
            return context.Files.FirstOrDefault(f => f.Id == Id);
        }

        public bool Remove(Guid Id)
        {
            var file = context.Files.FirstOrDefault(f => f.Id == Id);
            if (file != null)
            {
                try
                {
                    context.Files.Remove(file);
                    context.SaveChanges();
                    return true;
                }
                catch{}
            }
            return false;
        }

        public bool Save(FileInformation fileInfo)
        {
            try
            {
                context.Files.Add(fileInfo);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
