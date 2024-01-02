using AutoMapper;
using QuizMaster.API.Quiz.Models;
using QuizMaster.Library.Common.Entities.Questionnaire;

namespace QuizMaster.API.Quiz.Profiles
{
	public class CategoryProfile : Profile
	{
		public CategoryProfile()
		{
			CreateMap<QuestionCategory, CategoryDto>().ReverseMap();
			CreateMap<CategoryCreateDto, QuestionCategory>().ReverseMap();
		}
	}
}
