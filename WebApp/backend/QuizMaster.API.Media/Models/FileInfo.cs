namespace QuizMaster.API.Media.Models
{
    public class FileInfo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FileType { get; set; } = string.Empty;
        public double FileSize { get; set;} = 0;
        public string FileName { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
