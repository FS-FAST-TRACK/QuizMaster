using System.ComponentModel.DataAnnotations;

namespace QuizMaster.API.Media.Models
{
    public class FileInformation
    {
        [Key]
        public Guid Id { get; set; } = Guid.Empty;
        [Required]
        public string Type { get; set; } = string.Empty;
        [Required]
        public double Size { get; set;} = 0;
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public DateTime DateCreated { get; set; } = DateTime.Now;

        // default constructor
        public FileInformation() { }
    }
}
