using AutoMapper;
using QuizMaster.API.Quiz.Models;
using QuizMaster.API.Quiz.Protos;
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
			CreateMap<QuestionType, TypeReply>().ReverseMap();
			CreateMap<AddQuisTypeRequest, QuestionType>().ReverseMap();
		}
	}
}
