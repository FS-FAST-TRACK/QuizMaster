using System.ComponentModel.DataAnnotations;

namespace QuizMaster.API.Gateway.Models.System
{
    public class ReviewModel
    {
        public int? UserId { get; set; } = null;
        public int StarRating { get; set; } = 0;
        public string Comment { get; set; } = string.Empty;
    }

    public class Reviews : ReviewModel
    {
        [Key]
        public int Id { get; set; } = 0;
    }
}
