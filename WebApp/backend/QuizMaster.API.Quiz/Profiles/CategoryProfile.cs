using AutoMapper;
using QuizMaster.API.Quiz.Models;
using QuizMaster.Library.Common.Entities.Questionnaire;

namespace QuizMaster.API.Quiz.Profiles
{
	public class CategoryProfile : Profile
	{
		public CategoryProfile()
		{
			CreateMap<QuestionCategory, CategoryDto>().ForMember(destination => destination.DateCreated, act => act.Ignore()).ForMember(destination => destination.DateUpdated, act => act.Ignore());
			CreateMap<CategoryCreateDto, QuestionCategory>();
		}
	}
}
