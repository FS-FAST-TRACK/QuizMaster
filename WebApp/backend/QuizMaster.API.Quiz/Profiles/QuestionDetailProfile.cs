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
			CreateMap<QuestionDetailCreateDto, QuestionDetail>()
				.ForMember(x => x.DetailTypes, opt => opt.Ignore());

			CreateMap<QuestionDetail, QuestionDetailCreateDto>()
				.ForMember(x => x.DetailTypes, opt => opt.Ignore());

		}
	}
}
