using AutoMapper;
using QuizMaster.API.Quiz.Models;
using QuizMaster.API.Quiz.Protos;
using QuizMaster.Library.Common.Entities.Questionnaire;

namespace QuizMaster.API.Quiz.Profiles
{
	public class DifficultyProfile : Profile
	{
		public DifficultyProfile()
		{
			CreateMap<QuestionDifficulty, DifficultyDto>();
			CreateMap<DifficultyCreateDto, QuestionDifficulty>();
			CreateMap<QuestionDifficulty, CreateDifficultyRequest>().ReverseMap();
		}
	}
}
