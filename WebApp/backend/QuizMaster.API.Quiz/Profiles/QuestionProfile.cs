using AutoMapper;
using Newtonsoft.Json;
using QuizMaster.API.Quiz.Models;
using QuizMaster.Library.Common.Entities.Questionnaire;
using QuizMaster.Library.Common.Entities.Questionnaire.Answers;
using QuizMaster.Library.Common.Entities.Questionnaire.Details;
using QuizMaster.Library.Common.Entities.Questionnaire.Options;

namespace QuizMaster.API.Quiz.Profiles
{
	public class QuestionProfile: Profile
	{
		public QuestionProfile()
		{
			CreateMap<Question, QuestionDto>();
			CreateMap<QuestionCreateDto<string,string>, Question>();
			CreateMap<Question, QuestionCreateDto<string, string>>();

			// Custom Mapper for multiple choices question
			CreateMap<QuestionCreateDto<MultipleChoiceAnswer, MultipleChoiceQuestionDetail>, Question>()
				.ForMember(destination => destination.QAnswer, act => act.MapFrom(src=> JsonConvert.SerializeObject(src.QAnswer)));

			// Custom Mapper for true or false question
			CreateMap<QuestionCreateDto<bool, bool>, Question>()
				.ForMember(destination => destination.QAnswer, act => act.MapFrom(src => src.QAnswer));



			CreateMap<Question, QuestionCreateDto<string, string>>();
		}
	}
}
