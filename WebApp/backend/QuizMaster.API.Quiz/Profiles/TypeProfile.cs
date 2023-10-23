using AutoMapper;
using QuizMaster.API.Quiz.Models;
using QuizMaster.Library.Common.Entities.Questionnaire;

namespace QuizMaster.API.Quiz.Profiles
{
	public class TypeProfile : Profile
	{
		public TypeProfile()
		{
			CreateMap<QuestionType, TypeDto>();
			CreateMap<TypeCreateDto, QuestionType>();
			CreateMap<QuestionType, TypeCreateDto>();
		}
	}
}
