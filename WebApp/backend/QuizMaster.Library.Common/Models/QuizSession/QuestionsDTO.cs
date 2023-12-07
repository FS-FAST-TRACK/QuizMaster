using QuizMaster.Library.Common.Entities.Questionnaire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Library.Common.Models.QuizSession
{
    public class QuestionsDTO
    {
        public Question question { get; set; }
        public List<QuestionDetail> details { get; set; }
        public int RemainingTime { get; set; } = -1;
    }
}
