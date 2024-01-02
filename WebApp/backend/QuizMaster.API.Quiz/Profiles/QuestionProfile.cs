using AutoMapper;
using QuizMaster.API.Quiz.Models;
using QuizMaster.API.Quiz.SeedData;
using QuizMaster.Library.Common.Entities.Questionnaire;

namespace QuizMaster.API.Quiz.Profiles
{
	public class QuestionProfile: Profile
	{
		public QuestionProfile()
		{
			CreateMap<Question, QuestionDto>();
			CreateMap<Question, QuestionCreateDto>();
			CreateMap<QuestionCreateDto, Question>();



		}
	}
}
