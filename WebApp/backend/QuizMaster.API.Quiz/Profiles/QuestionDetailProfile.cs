using AutoMapper;
using QuizMaster.API.Quiz.Models;
using QuizMaster.Library.Common.Entities.Questionnaire;

namespace QuizMaster.API.Quiz.Profiles
{
	public class QuestionDetailProfile: Profile
	{
        public QuestionDetailProfile()
        {
			CreateMap<QuestionDetail, QuestionDetailDto>();

		}
	}
}
