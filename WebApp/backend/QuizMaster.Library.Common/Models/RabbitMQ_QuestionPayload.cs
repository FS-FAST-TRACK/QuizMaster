using QuizMaster.Library.Common.Entities.Questionnaire;

namespace QuizMaster.Library.Common.Models
{
    public class RabbitMQ_QuestionPayload
    {
        public IEnumerable<Question> Questions { get; set; } = new List<Question>();
        public IEnumerable<DetailType> DetailTypes { get; set; } = new List<DetailType>();
        public IEnumerable<QuestionCategory> QuestionCategories { get; set; } = new List<QuestionCategory>();
        public IEnumerable<QuestionDetail> QuestionDetails { get; set; } = new List<QuestionDetail>();
        public IEnumerable<QuestionDifficulty> QuestionDifficulties { get; set; } = new List<QuestionDifficulty>();
        public IEnumerable<QuestionDetailType> QuestionDetailTypes { get; set; } = new List<QuestionDetailType>();
        public IEnumerable<QuestionType> QuestionTypes { get; set; } = new List<QuestionType>();
    }
}
